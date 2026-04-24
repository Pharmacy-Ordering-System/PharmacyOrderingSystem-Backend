using Microsoft.EntityFrameworkCore;
using PharmacyOrderingWebsite.Data;
using PharmacyOrderingWebsite.DTOs;
using PharmacyOrderingWebsite.Enums;
using PharmacyOrderingWebsite.Models;
using PharmacyOrderingWebsite.Services;

namespace PharmacyOrderingWebsite.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        public OrderService(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<Order> CreateOrder(OrderDto dto, int userId)
        {

            try
            {
                Console.WriteLine("[ORDER] Creating order...");

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    throw new Exception("User not found");

                var order = new Order
                {
                    UserId = userId,
                    Status = OrderStatus.Placed,
                    TotalAmount = 0,
                    Items = new List<OrderItem>()
                };

                using var transaction = await _context.Database.BeginTransactionAsync();

                foreach (var item in dto.Items)
                {
                    if (item.Quantity <= 0)
                        throw new Exception("Invalid quantity");

                    var medicine = await _context.Medicines.FindAsync(item.MedicineId);
                    if (medicine == null)
                        throw new Exception($"Medicine not found: {item.MedicineId}");

                    var inventory = await _context.Inventories
                        .FirstOrDefaultAsync(i => i.MedicineId == item.MedicineId);

                    if (inventory == null)
                        throw new Exception("Inventory not found");

                    if (inventory.Stock < item.Quantity)
                        throw new Exception("Not enough stock");

                    inventory.Stock -= item.Quantity;

                    order.Items.Add(new OrderItem
                    {
                        MedicineId = item.MedicineId,
                        Quantity = item.Quantity,
                    });

                    order.TotalAmount += medicine.Price * item.Quantity;
                }

                _context.Orders.Add(order);

                // loyalty BEFORE save
                var loyalty = await _context.LoyaltyPoints
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                if (loyalty != null)
                {
                    loyalty.Points += 10;
                }
                else
                {
                    loyalty = new LoyaltyPoints
                    {
                        UserId = userId,
                        Points = 10
                    };
                    _context.LoyaltyPoints.Add(loyalty);
                }

                // single save
                await _context.SaveChangesAsync();

                // commit transaction
                await transaction.CommitAsync();


                try
                {
                    await _emailService.SendEmailAsync(
                        user.Email,
                        "Order Confirmed",
                        $"Your order #{order.Id} has been placed successfully."
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[EMAIL ERROR] " + ex.Message);

                }

                return order;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ORDER ERROR] {ex.Message}");
                throw;
            }
        }

        public async Task<Order> UpdateOrderStatus(int orderId, OrderStatus status)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new Exception("Order not found");

            order.Status = status;

            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<List<Order>> GetAll()
        {
            return await _context.Orders
                .Include(o => o.Items)
                .ToListAsync();
        }
    }
}