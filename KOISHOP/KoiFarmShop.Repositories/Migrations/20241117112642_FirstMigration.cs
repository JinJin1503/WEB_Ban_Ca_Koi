using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KoiFarmShop.Repositories.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KoiCategories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(nullable: true),
                    CategoryDesc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KoiCategories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    PromotionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromotionName = table.Column<string>(nullable: true),
                    DiscountRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.PromotionId);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportDate = table.Column<DateTime>(nullable: false),
                    TotalRevenue = table.Column<int>(type: "int", nullable: false),
                    TotalCustomers = table.Column<int>(nullable: false),
                    Summary = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(nullable: true),
                    PasswordHasher = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "KoiFishs",
                columns: table => new
                {
                    KoiId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KoiName = table.Column<string>(nullable: true),
                    Origin = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Size = table.Column<float>(nullable: false),
                    BreedType = table.Column<string>(nullable: true),
                    Personality = table.Column<string>(nullable: true),
                    DailyFeed = table.Column<int>(nullable: false),
                    ScreeningRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    HealthStatus = table.Column<string>(nullable: true),
                    Awards = table.Column<string>(nullable: true),
                    PricePerKoi = table.Column<int>(type: "int", nullable: false),
                    PricePerBatch = table.Column<int>(type: "int", nullable: false),
                    ImageURL = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KoiFishs", x => x.KoiId);
                    table.ForeignKey(
                        name: "FK_KoiFishs_KoiCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "KoiCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    RegistrationDate = table.Column<DateTime>(nullable: false),
                    Points = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Customers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    StaffId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    JoinDate = table.Column<DateTime>(nullable: false),
                    Salary = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.StaffId);
                    table.ForeignKey(
                        name: "FK_Staffs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CareServices",
                columns: table => new
                {
                    ServiceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceDesc = table.Column<string>(nullable: true),
                    ServicePrice = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    KoiId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareServices", x => x.ServiceId);
                    table.ForeignKey(
                        name: "FK_CareServices_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CareServices_KoiFishs_KoiId",
                        column: x => x.KoiId,
                        principalTable: "KoiFishs",
                        principalColumn: "KoiId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    CartId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_Carts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConsignmentRequests",
                columns: table => new
                {
                    RequestId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    ConsignmentFee = table.Column<int>(type: "int", nullable: false),
                    ConsignmentType = table.Column<string>(nullable: true),
                    Certificate = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsignmentRequests", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_ConsignmentRequests_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    FeedbackId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    FeedbackDate = table.Column<DateTime>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    KoiId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.FeedbackId);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feedbacks_KoiFishs_KoiId",
                        column: x => x.KoiId,
                        principalTable: "KoiFishs",
                        principalColumn: "KoiId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    StaffId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Staffs_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staffs",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    CartItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuantityPerKoi = table.Column<int>(nullable: false),
                    QuantityPerBatch = table.Column<int>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    CartId = table.Column<int>(nullable: false),
                    KoiId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.CartItemId);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "CartId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_KoiFishs_KoiId",
                        column: x => x.KoiId,
                        principalTable: "KoiFishs",
                        principalColumn: "KoiId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConsignmentKois",
                columns: table => new
                {
                    ConsignmentKoiId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgreedPrice = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    RequestId = table.Column<int>(nullable: false),
                    ConsignmentRequestRequestId = table.Column<int>(nullable: true),
                    KoiId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsignmentKois", x => x.ConsignmentKoiId);
                    table.ForeignKey(
                        name: "FK_ConsignmentKois_ConsignmentRequests_ConsignmentRequestRequestId",
                        column: x => x.ConsignmentRequestRequestId,
                        principalTable: "ConsignmentRequests",
                        principalColumn: "RequestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsignmentKois_KoiFishs_KoiId",
                        column: x => x.KoiId,
                        principalTable: "KoiFishs",
                        principalColumn: "KoiId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuantityPerKoi = table.Column<int>(nullable: false),
                    QuantityPerBatch = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    PaymentMethod = table.Column<string>(nullable: true),
                    ShippingAddress = table.Column<string>(nullable: true),
                    OrderId = table.Column<int>(nullable: false),
                    KoiId = table.Column<int>(nullable: false),
                    PromotionId = table.Column<int>(nullable: false),
                    TotalAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetails_KoiFishs_KoiId",
                        column: x => x.KoiId,
                        principalTable: "KoiFishs",
                        principalColumn: "KoiId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "PromotionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "KoiCategories",
                columns: new[] { "CategoryId", "CategoryDesc", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Koi Kohaku thân đỏ trắng là dòng cá tiêu biểu, khả năng sinh sôi nảy nở cực tốt và cũng là 1 trong những dòng cá có tuổi thọ cao.", "Koi Kohaku" },
                    { 2, "Koi Karashi loại cá Koi này có da trơn màu sắc từ màu vàng nhạt đến vàng đậm toàn thân, rất đẹp và tươi tắn.", "Koi Karashi" },
                    { 3, "Koi Showa là dòng Gosanke,Showa hấp dẫn người chơi bởi 3 màu đen-đỏ-trắng. Trong đó, màu trắng(Shiroji) là màu nền, tiếp theo là màu đỏ(Hi) và màu đen(Sumi).", "Koi Showa" },
                    { 4, "Koi Asagi có màu xám bạc, chính giữa có vẩy đen, ngoài bìa mỗi vẩy hơi xanh dậm da trời, nhìn như lưới cá Fukurin.Nửa phần dưới bụng cá là màu cam.", "Koi Asagi" },
                    { 5, "Koi Shusui tạo cảm giác mạnh từ 3 màu đen, đỏ, trắng tương phản cùng với 2 dải vảy đen nháy cùng màu đối xứng lợp mái ấn tượng trên sống lưng.", "Koi Shusui" }
                });

            migrationBuilder.InsertData(
                table: "Promotions",
                columns: new[] { "PromotionId", "Description", "DiscountRate", "EndDate", "PromotionName", "StartDate" },
                values: new object[,]
                {
                    { 1, "Giảm giá 10% cho tất cả các sản phẩm trong mùa hè.", 0.10m, new DateTime(2024, 12, 17, 18, 26, 42, 616, DateTimeKind.Local).AddTicks(2041), "Khuyến mãi mùa hè", new DateTime(2024, 11, 17, 18, 26, 42, 616, DateTimeKind.Local).AddTicks(1903) },
                    { 2, "Giảm giá 15% cho các đơn hàng trên 5 triệu.", 0.15m, new DateTime(2024, 11, 27, 18, 26, 42, 616, DateTimeKind.Local).AddTicks(2378), "Khuyến mãi Tết Nguyên Đán", new DateTime(2024, 11, 7, 18, 26, 42, 616, DateTimeKind.Local).AddTicks(2367) }
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "ReportId", "ReportDate", "Summary", "TotalCustomers", "TotalRevenue" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 11, 17, 18, 26, 42, 616, DateTimeKind.Local).AddTicks(2826), "Doanh thu tháng này tăng 10% so với tháng trước.", 100, 5000000 },
                    { 2, new DateTime(2024, 10, 17, 18, 26, 42, 616, DateTimeKind.Local).AddTicks(3308), "Doanh thu tháng trước ổn định.", 80, 4500000 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "PasswordHasher", "UserName" },
                values: new object[,]
                {
                    { 1, "Khanhng5776@ut.edu.vn", "15032005", "ngogiakhanh" },
                    { 2, "dihny5348@ut.edu.vn", "19072005", "huynhngoyendi" },
                    { 3, "giakhanhngo1503@gmail.com", "sangyoonjin", "sangyoonjin" },
                    { 4, "yendi1907@gmai.com", "yendi", "yendi" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Address", "CustomerName", "Email", "Phone", "Points", "RegistrationDate", "UserId" },
                values: new object[,]
                {
                    { 2, "456 Đường DEF, Quận 2", "Yến Di", "yendi1907@gmai.com", "0987654321", 200, new DateTime(2024, 11, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(2521), 4 },
                    { 1, "123 Đường ABC, Quận 1", "Sang Yoon Jin", "giakhanhngo1503@gmail.com", "0123456789", 100, new DateTime(2024, 11, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(2115), 3 }
                });

            migrationBuilder.InsertData(
                table: "KoiFishs",
                columns: new[] { "KoiId", "Age", "Awards", "BreedType", "CategoryId", "DailyFeed", "Gender", "HealthStatus", "ImageURL", "KoiName", "Origin", "Personality", "PricePerBatch", "PricePerKoi", "ScreeningRate", "Size" },
                values: new object[,]
                {
                    { 25, 4, "Giải nhất", "Thuần Việt", 5, 250, "Mái", "Khỏe mạnh", "/images/Shusui5 .jpg", "Koi Shusui 5", " Việt Nam", "Hiền hòa", 7500000, 1500000, 96.5m, 34f },
                    { 24, 3, "Giải ba", "Thuần Việt", 5, 210, "Đực", "Khỏe mạnh", "/images/Shusui 4.jpg", "Koi Shusui 4", "Nhật Bản", "Chủ động", 7000000, 1400000, 95.0m, 31f },
                    { 23, 2, "Giải nhất", "Thuần Việt", 5, 200, "Mái", "Khỏe mạnh", "/images/Shusui 3.jpg", "Koi Shusui 3", "Việt Nam", "Hiền hòa", 6750000, 1350000, 94.5m, 28f },
                    { 22, 4, "Giải nhì", "Lai F1", 5, 240, "Đực", "Khỏe mạnh", "/images/Shusui 2.jpg", "Koi Shusui 2", "Nhật Bản", "Chủ động", 7500000, 1500000, 96.0m, 33f },
                    { 21, 3, "Giải ba", "Nhập khẩu", 5, 220, "Mái", "Khỏe mạnh", "/images/Shusui1.jpg", "Koi Shusui 1", "Việt Nam", "Hiền hòa", 7000000, 1400000, 95.0m, 30f },
                    { 20, 4, "Giải nhất", "Nhập khẩu", 4, 250, "Mái", "Khỏe mạnh", "/images/Asagi5.jpg", "Koi Asagi 5", "Việt Nam", "Hiền hòa", 7500000, 1500000, 96.5m, 36f },
                    { 19, 3, "Giải ba", "Lai F1", 4, 220, "Đực", "Khỏe mạnh", "/images/Asagi 4.jpg", "Koi Asagi 4", "Nhật Bản", "Chủ động", 7250000, 1450000, 95.5m, 32f },
                    { 18, 2, "Giải nhất", "Thuần Việt", 4, 200, "Mái", "Khỏe mạnh", "/images/Asagi 3.jpg", "Koi Asagi 3", "Việt Nam", "Hiền hòa", 7000000, 1400000, 94.5m, 29f },
                    { 17, 4, "Giải nhì", "Thuần Việt", 4, 240, "Đực", "Khỏe mạnh", "/images/Asagi 2.jpg", "Koi Asagi 2", "Nhật Bản", "Chủ động", 7500000, 1500000, 96.0m, 34f },
                    { 16, 3, "Giải ba", "Nhập khẩu", 4, 210, "Mái", "Khỏe mạnh", "/images/Asagi 1.jpg", "Koi Asagi 1", "Việt Nam", "Hiền hòa", 7250000, 1450000, 95.0m, 31f },
                    { 14, 3, "Giải ba", "Thuần Việt", 3, 220, "Đực", "Khỏe mạnh", "/images/Showa 4.jpg", "Koi Showa 4", "Nhật Bản", "Chủ động", 7500000, 1500000, 96.5m, 32f },
                    { 15, 4, "Giải nhất", "Thuần Việt", 3, 260, "Mái", "Khỏe mạnh", "/images/Showa5.jpg", "Koi Showa 5", "Việt Nam", "Hiền hòa", 7750000, 1550000, 97.0m, 34f },
                    { 12, 4, "Giải nhì", "Nhập khẩu", 3, 250, "Đực", "Khỏe mạnh", "/images/Showa2.jpg", "Koi Showa 2", "Nhật Bản", "Chủ động", 7250000, 1450000, 96.0m, 35f },
                    { 11, 3, "Giải ba", "Thuần Việt", 3, 210, "Mái", "Khỏe mạnh", "/images/Showa 1.jpg", "Koi Showa 1", "Việt Nam", "Hiền hòa", 7000000, 1400000, 95.0m, 33f },
                    { 10, 5, "Giải nhất", "Nhập khẩu", 2, 300, "Đực", "Khỏe mạnh", "/images/Koi Karashi 5 .jpg", "Koi Karashi 5", "Việt Nam", "Chủ động", 7500000, 1500000, 97.5m, 35f },
                    { 9, 4, "Giải ba", "Thuần Việt", 2, 240, "Mái", "Khỏe mạnh", "/images/Koi Karashi 4.jpg", "Koi Karashi 4", "Nhật Bản", "Hiền hòa", 7250000, 1450000, 96.0m, 32f },
                    { 8, 2, "Giải nhì", "Lai F1", 2, 190, "Đực", "Khỏe mạnh", "/images/Koi Karashi 3.jpg", "Koi Karashi 3", "Việt Nam", "Chủ động", 6750000, 1350000, 94.0m, 27f },
                    { 7, 3, "Giải nhất", "Thuần Việt", 2, 230, "Mái", "Khỏe mạnh", "/images/Koi Karashi2.jpg", "Koi Karashi 2", "Nhật Bản", "Hiền hòa", 7000000, 1400000, 95.5m, 30f },
                    { 6, 2, "Giải ba", "Lai F1", 2, 210, "Đực", "Khỏe mạnh", "/images/Koi Karashi 1.jpg", "Koi Karashi 1", "Việt Nam", "Chủ động", 6500000, 1300000, 94.5m, 28f },
                    { 5, 3, "Giải nhì", "Thuần Việt", 1, 220, "Mái", "Khỏe mạnh", "/images/Kohaku5.jpg", "Koi Kohaku 5", "Việt Nam", "Hiền hòa", 7500000, 1500000, 96.5m, 30f },
                    { 4, 5, "Giải nhất", "Lai F1", 1, 300, "Đực", "Khỏe mạnh", "/images/Kohaku4 .jpg", "Koi Kohaku 4", "Nhật Bản", "Chủ động", 8500000, 1700000, 98.0m, 35f },
                    { 3, 2, "Giải ba", "Thuần Việt", 1, 180, "Mái", "Khỏe mạnh", "/images/Kohaku3.jpg", "Koi Kohaku 3", "Việt Nam", "Chủ động", 7000000, 1400000, 94.0m, 28f },
                    { 2, 4, "Giải nhì", "Thuần Việt", 1, 250, "Đực", "Khỏe mạnh", "/images/Kohaku2.jpg", "Koi Kohaku 2", "Nhật Bản", "Hiền hòa", 8000000, 1600000, 96.0m, 32f },
                    { 13, 2, "Giải nhất", "Lai F1", 3, 200, "Mái", "Khỏe mạnh", "/images/Showa 3.jpg", "Koi Showa 3", "Việt Nam", "Hiền hòa", 6750000, 1350000, 94.5m, 30f },
                    { 1, 3, "Giải nhất", "Nhập khẩu", 1, 200, "Mái", "Khỏe mạnh", "/images/Kohaku1.jpg", "Koi Kohaku 1", "Việt Nam", "Hiền hòa", 7500000, 1500000, 95.5m, 30f }
                });

            migrationBuilder.InsertData(
                table: "Staffs",
                columns: new[] { "StaffId", "Email", "JoinDate", "Phone", "Role", "Salary", "StaffName", "UserId" },
                values: new object[,]
                {
                    { 1, "dihny5348@ut.edu.vn", new DateTime(2022, 11, 17, 18, 26, 42, 613, DateTimeKind.Local).AddTicks(9626), "0905681918", "Quản lý", 20000000, "Ngô Gia Khánh", 1 },
                    { 2, "Khanhng5776@ut.edu.vn", new DateTime(2023, 11, 17, 18, 26, 42, 614, DateTimeKind.Local).AddTicks(7994), "0344883755", "Nhân viên bán hàng", 15000000, "Huỳnh Ngô Yến Di", 2 }
                });

            migrationBuilder.InsertData(
                table: "CareServices",
                columns: new[] { "ServiceId", "CustomerId", "KoiId", "ServiceDesc", "ServicePrice" },
                values: new object[,]
                {
                    { 1, 1, 1, "Chăm sóc cá Koi tiêu chuẩn", 150000 },
                    { 2, 2, 2, "Chăm sóc cá Koi nâng cao", 200000 }
                });

            migrationBuilder.InsertData(
                table: "Carts",
                columns: new[] { "CartId", "CustomerId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "ConsignmentKois",
                columns: new[] { "ConsignmentKoiId", "AgreedPrice", "ConsignmentRequestRequestId", "EndDate", "KoiId", "RequestId", "StartDate" },
                values: new object[,]
                {
                    { 1, 150000, null, new DateTime(2025, 5, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(4502), 1, 1, new DateTime(2024, 11, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(4366) },
                    { 2, 200000, null, new DateTime(2025, 11, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(4868), 2, 2, new DateTime(2024, 11, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(4861) }
                });

            migrationBuilder.InsertData(
                table: "ConsignmentRequests",
                columns: new[] { "RequestId", "Certificate", "ConsignmentFee", "ConsignmentType", "CustomerId", "Notes", "RequestDate", "Status" },
                values: new object[,]
                {
                    { 1, "Giấy chứng nhận 001", 50000, "Bán", 1, "Yêu cầu bán cá Koi loại A", new DateTime(2024, 11, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(2972), "Đang xử lý" },
                    { 2, "Giấy chứng nhận 002", 70000, "Chăm sóc", 2, "Chăm sóc cá Koi loại B", new DateTime(2024, 11, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(3868), "Hoàn thành" }
                });

            migrationBuilder.InsertData(
                table: "Feedbacks",
                columns: new[] { "FeedbackId", "Comment", "CustomerId", "FeedbackDate", "KoiId", "Rating" },
                values: new object[,]
                {
                    { 1, "Cá Koi đẹp và khỏe mạnh", 1, new DateTime(2024, 11, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(5507), 1, 5 },
                    { 2, "Dịch vụ chăm sóc tốt, nhưng giao hàng chậm", 2, new DateTime(2024, 11, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(5874), 2, 4 }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "CustomerId", "OrderDate", "StaffId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 11, 17, 18, 26, 42, 616, DateTimeKind.Local).AddTicks(824), 1 },
                    { 2, 2, new DateTime(2024, 11, 16, 18, 26, 42, 616, DateTimeKind.Local).AddTicks(1204), 2 }
                });

            migrationBuilder.InsertData(
                table: "CartItems",
                columns: new[] { "CartItemId", "CartId", "DateAdded", "KoiId", "QuantityPerBatch", "QuantityPerKoi" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 11, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(750), 1, 1, 2 },
                    { 2, 2, new DateTime(2024, 11, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(1161), 2, 2, 3 }
                });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "OrderDetailId", "KoiId", "OrderId", "PaymentMethod", "PromotionId", "QuantityPerBatch", "QuantityPerKoi", "ShippingAddress", "Status", "TotalAmount" },
                values: new object[,]
                {
                    { 1, 1, 1, "Chuyển khoản", 1, 1, 3, "123 Main St", "Đang xử lý", 12000000 },
                    { 2, 2, 2, "Thanh toán khi nhận hàng", 2, 1, 2, "456 Second St", "Đã giao", 11200000 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CareServices_CustomerId",
                table: "CareServices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CareServices_KoiId",
                table: "CareServices",
                column: "KoiId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_KoiId",
                table: "CartItems",
                column: "KoiId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CustomerId",
                table: "Carts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsignmentKois_ConsignmentRequestRequestId",
                table: "ConsignmentKois",
                column: "ConsignmentRequestRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsignmentKois_KoiId",
                table: "ConsignmentKois",
                column: "KoiId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsignmentRequests_CustomerId",
                table: "ConsignmentRequests",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                table: "Customers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_CustomerId",
                table: "Feedbacks",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_KoiId",
                table: "Feedbacks",
                column: "KoiId");

            migrationBuilder.CreateIndex(
                name: "IX_KoiFishs_CategoryId",
                table: "KoiFishs",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_KoiId",
                table: "OrderDetails",
                column: "KoiId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_PromotionId",
                table: "OrderDetails",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StaffId",
                table: "Orders",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_UserId",
                table: "Staffs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CareServices");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "ConsignmentKois");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "ConsignmentRequests");

            migrationBuilder.DropTable(
                name: "KoiFishs");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropTable(
                name: "KoiCategories");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Staffs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
