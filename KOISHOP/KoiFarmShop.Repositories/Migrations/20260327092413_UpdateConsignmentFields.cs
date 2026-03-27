using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KoiFarmShop.Repositories.Migrations
{
    public partial class UpdateConsignmentFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CareService",
                table: "ConsignmentRequests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConsignmentDate",
                table: "ConsignmentRequests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ConsignmentDuration",
                table: "ConsignmentRequests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "KoiAge",
                table: "ConsignmentRequests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "KoiBreed",
                table: "ConsignmentRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KoiImage",
                table: "ConsignmentRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KoiName",
                table: "ConsignmentRequests",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "KoiSize",
                table: "ConsignmentRequests",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "ConsignmentRequests",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CartItems",
                keyColumn: "CartItemId",
                keyValue: 1,
                column: "DateAdded",
                value: new DateTime(2026, 3, 27, 16, 24, 13, 172, DateTimeKind.Local).AddTicks(9796));

            migrationBuilder.UpdateData(
                table: "CartItems",
                keyColumn: "CartItemId",
                keyValue: 2,
                column: "DateAdded",
                value: new DateTime(2026, 3, 27, 16, 24, 13, 173, DateTimeKind.Local).AddTicks(92));

            migrationBuilder.UpdateData(
                table: "ConsignmentKois",
                keyColumn: "ConsignmentKoiId",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 9, 27, 16, 24, 13, 173, DateTimeKind.Local).AddTicks(2402), new DateTime(2026, 3, 27, 16, 24, 13, 173, DateTimeKind.Local).AddTicks(2305) });

            migrationBuilder.UpdateData(
                table: "ConsignmentKois",
                keyColumn: "ConsignmentKoiId",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2027, 3, 27, 16, 24, 13, 173, DateTimeKind.Local).AddTicks(2692), new DateTime(2026, 3, 27, 16, 24, 13, 173, DateTimeKind.Local).AddTicks(2691) });

            migrationBuilder.UpdateData(
                table: "ConsignmentRequests",
                keyColumn: "RequestId",
                keyValue: 1,
                column: "RequestDate",
                value: new DateTime(2026, 3, 27, 16, 24, 13, 173, DateTimeKind.Local).AddTicks(1296));

            migrationBuilder.UpdateData(
                table: "ConsignmentRequests",
                keyColumn: "RequestId",
                keyValue: 2,
                column: "RequestDate",
                value: new DateTime(2026, 3, 27, 16, 24, 13, 173, DateTimeKind.Local).AddTicks(1934));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1,
                column: "RegistrationDate",
                value: new DateTime(2026, 3, 27, 16, 24, 13, 173, DateTimeKind.Local).AddTicks(773));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2,
                column: "RegistrationDate",
                value: new DateTime(2026, 3, 27, 16, 24, 13, 173, DateTimeKind.Local).AddTicks(1050));

            migrationBuilder.UpdateData(
                table: "Feedbacks",
                keyColumn: "FeedbackId",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2026, 3, 27, 16, 24, 13, 173, DateTimeKind.Local).AddTicks(3423));

            migrationBuilder.UpdateData(
                table: "Feedbacks",
                keyColumn: "FeedbackId",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2026, 3, 27, 16, 24, 13, 173, DateTimeKind.Local).AddTicks(3692));

            migrationBuilder.UpdateData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 1,
                column: "TotalAmount",
                value: 12000000);

            migrationBuilder.UpdateData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 2,
                column: "TotalAmount",
                value: 11200000);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2026, 3, 27, 16, 24, 13, 173, DateTimeKind.Local).AddTicks(8332));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 2,
                column: "OrderDate",
                value: new DateTime(2026, 3, 26, 16, 24, 13, 173, DateTimeKind.Local).AddTicks(8642));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "PromotionId",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 4, 27, 16, 24, 13, 173, DateTimeKind.Local).AddTicks(9254), new DateTime(2026, 3, 27, 16, 24, 13, 173, DateTimeKind.Local).AddTicks(9159) });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "PromotionId",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 4, 6, 16, 24, 13, 173, DateTimeKind.Local).AddTicks(9445), new DateTime(2026, 3, 17, 16, 24, 13, 173, DateTimeKind.Local).AddTicks(9441) });

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "ReportId",
                keyValue: 1,
                column: "ReportDate",
                value: new DateTime(2026, 3, 27, 16, 24, 13, 173, DateTimeKind.Local).AddTicks(9727));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "ReportId",
                keyValue: 2,
                column: "ReportDate",
                value: new DateTime(2026, 2, 27, 16, 24, 13, 174, DateTimeKind.Local).AddTicks(92));

            migrationBuilder.UpdateData(
                table: "Staffs",
                keyColumn: "StaffId",
                keyValue: 1,
                column: "JoinDate",
                value: new DateTime(2024, 3, 27, 16, 24, 13, 171, DateTimeKind.Local).AddTicks(6343));

            migrationBuilder.UpdateData(
                table: "Staffs",
                keyColumn: "StaffId",
                keyValue: 2,
                column: "JoinDate",
                value: new DateTime(2025, 3, 27, 16, 24, 13, 172, DateTimeKind.Local).AddTicks(7812));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CareService",
                table: "ConsignmentRequests");

            migrationBuilder.DropColumn(
                name: "ConsignmentDate",
                table: "ConsignmentRequests");

            migrationBuilder.DropColumn(
                name: "ConsignmentDuration",
                table: "ConsignmentRequests");

            migrationBuilder.DropColumn(
                name: "KoiAge",
                table: "ConsignmentRequests");

            migrationBuilder.DropColumn(
                name: "KoiBreed",
                table: "ConsignmentRequests");

            migrationBuilder.DropColumn(
                name: "KoiImage",
                table: "ConsignmentRequests");

            migrationBuilder.DropColumn(
                name: "KoiName",
                table: "ConsignmentRequests");

            migrationBuilder.DropColumn(
                name: "KoiSize",
                table: "ConsignmentRequests");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "ConsignmentRequests");

            migrationBuilder.UpdateData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 1,
                column: "TotalAmount",
                value: 12000000);

            migrationBuilder.UpdateData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 2,
                column: "TotalAmount",
                value: 11200000);

            migrationBuilder.UpdateData(
                table: "CartItems",
                keyColumn: "CartItemId",
                keyValue: 1,
                column: "DateAdded",
                value: new DateTime(2024, 11, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(750));

            migrationBuilder.UpdateData(
                table: "CartItems",
                keyColumn: "CartItemId",
                keyValue: 2,
                column: "DateAdded",
                value: new DateTime(2024, 11, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(1161));

            migrationBuilder.UpdateData(
                table: "ConsignmentKois",
                keyColumn: "ConsignmentKoiId",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(4502), new DateTime(2024, 11, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(4366) });

            migrationBuilder.UpdateData(
                table: "ConsignmentKois",
                keyColumn: "ConsignmentKoiId",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 11, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(4868), new DateTime(2024, 11, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(4861) });

            migrationBuilder.UpdateData(
                table: "ConsignmentRequests",
                keyColumn: "RequestId",
                keyValue: 1,
                column: "RequestDate",
                value: new DateTime(2024, 11, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(2972));

            migrationBuilder.UpdateData(
                table: "ConsignmentRequests",
                keyColumn: "RequestId",
                keyValue: 2,
                column: "RequestDate",
                value: new DateTime(2024, 11, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(3868));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1,
                column: "RegistrationDate",
                value: new DateTime(2024, 11, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(2115));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2,
                column: "RegistrationDate",
                value: new DateTime(2024, 11, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(2521));

            migrationBuilder.UpdateData(
                table: "Feedbacks",
                keyColumn: "FeedbackId",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 11, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(5507));

            migrationBuilder.UpdateData(
                table: "Feedbacks",
                keyColumn: "FeedbackId",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 11, 17, 18, 26, 42, 615, DateTimeKind.Local).AddTicks(5874));

            migrationBuilder.UpdateData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 1,
                column: "TotalAmount",
                value: 0);

            migrationBuilder.UpdateData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 2,
                column: "TotalAmount",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2024, 11, 17, 18, 26, 42, 616, DateTimeKind.Local).AddTicks(824));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 2,
                column: "OrderDate",
                value: new DateTime(2024, 11, 16, 18, 26, 42, 616, DateTimeKind.Local).AddTicks(1204));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "PromotionId",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 12, 17, 18, 26, 42, 616, DateTimeKind.Local).AddTicks(2041), new DateTime(2024, 11, 17, 18, 26, 42, 616, DateTimeKind.Local).AddTicks(1903) });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "PromotionId",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 11, 27, 18, 26, 42, 616, DateTimeKind.Local).AddTicks(2378), new DateTime(2024, 11, 7, 18, 26, 42, 616, DateTimeKind.Local).AddTicks(2367) });

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "ReportId",
                keyValue: 1,
                column: "ReportDate",
                value: new DateTime(2024, 11, 17, 18, 26, 42, 616, DateTimeKind.Local).AddTicks(2826));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "ReportId",
                keyValue: 2,
                column: "ReportDate",
                value: new DateTime(2024, 10, 17, 18, 26, 42, 616, DateTimeKind.Local).AddTicks(3308));

            migrationBuilder.UpdateData(
                table: "Staffs",
                keyColumn: "StaffId",
                keyValue: 1,
                column: "JoinDate",
                value: new DateTime(2022, 11, 17, 18, 26, 42, 613, DateTimeKind.Local).AddTicks(9626));

            migrationBuilder.UpdateData(
                table: "Staffs",
                keyColumn: "StaffId",
                keyValue: 2,
                column: "JoinDate",
                value: new DateTime(2023, 11, 17, 18, 26, 42, 614, DateTimeKind.Local).AddTicks(7994));
        }
    }
}
