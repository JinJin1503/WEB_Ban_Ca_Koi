using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KoiFarmShop.Repositories.Migrations
{
    public partial class AddLockoutColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FailedAttemptCount",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Orders",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CartItems",
                keyColumn: "CartItemId",
                keyValue: 1,
                column: "DateAdded",
                value: new DateTime(2026, 4, 5, 1, 14, 21, 909, DateTimeKind.Local).AddTicks(7024));

            migrationBuilder.UpdateData(
                table: "CartItems",
                keyColumn: "CartItemId",
                keyValue: 2,
                column: "DateAdded",
                value: new DateTime(2026, 4, 5, 1, 14, 21, 909, DateTimeKind.Local).AddTicks(7550));

            migrationBuilder.UpdateData(
                table: "ConsignmentKois",
                keyColumn: "ConsignmentKoiId",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 10, 5, 1, 14, 21, 910, DateTimeKind.Local).AddTicks(283), new DateTime(2026, 4, 5, 1, 14, 21, 910, DateTimeKind.Local).AddTicks(163) });

            migrationBuilder.UpdateData(
                table: "ConsignmentKois",
                keyColumn: "ConsignmentKoiId",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2027, 4, 5, 1, 14, 21, 910, DateTimeKind.Local).AddTicks(674), new DateTime(2026, 4, 5, 1, 14, 21, 910, DateTimeKind.Local).AddTicks(673) });

            migrationBuilder.UpdateData(
                table: "ConsignmentRequests",
                keyColumn: "RequestId",
                keyValue: 1,
                column: "RequestDate",
                value: new DateTime(2026, 4, 5, 1, 14, 21, 909, DateTimeKind.Local).AddTicks(9047));

            migrationBuilder.UpdateData(
                table: "ConsignmentRequests",
                keyColumn: "RequestId",
                keyValue: 2,
                column: "RequestDate",
                value: new DateTime(2026, 4, 5, 1, 14, 21, 909, DateTimeKind.Local).AddTicks(9747));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1,
                column: "RegistrationDate",
                value: new DateTime(2026, 4, 5, 1, 14, 21, 909, DateTimeKind.Local).AddTicks(8375));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2,
                column: "RegistrationDate",
                value: new DateTime(2026, 4, 5, 1, 14, 21, 909, DateTimeKind.Local).AddTicks(8679));

            migrationBuilder.UpdateData(
                table: "Feedbacks",
                keyColumn: "FeedbackId",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2026, 4, 5, 1, 14, 21, 910, DateTimeKind.Local).AddTicks(1301));

            migrationBuilder.UpdateData(
                table: "Feedbacks",
                keyColumn: "FeedbackId",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2026, 4, 5, 1, 14, 21, 910, DateTimeKind.Local).AddTicks(1655));

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
                value: new DateTime(2026, 4, 5, 1, 14, 21, 911, DateTimeKind.Local).AddTicks(3315));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 2,
                column: "OrderDate",
                value: new DateTime(2026, 4, 4, 1, 14, 21, 911, DateTimeKind.Local).AddTicks(3853));

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "PromotionId",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 5, 5, 1, 14, 21, 911, DateTimeKind.Local).AddTicks(5043), new DateTime(2026, 4, 5, 1, 14, 21, 911, DateTimeKind.Local).AddTicks(4886) });

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "PromotionId",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 4, 15, 1, 14, 21, 911, DateTimeKind.Local).AddTicks(5322), new DateTime(2026, 3, 26, 1, 14, 21, 911, DateTimeKind.Local).AddTicks(5317) });

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "ReportId",
                keyValue: 1,
                column: "ReportDate",
                value: new DateTime(2026, 4, 5, 1, 14, 21, 911, DateTimeKind.Local).AddTicks(5776));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "ReportId",
                keyValue: 2,
                column: "ReportDate",
                value: new DateTime(2026, 3, 5, 1, 14, 21, 911, DateTimeKind.Local).AddTicks(6362));

            migrationBuilder.UpdateData(
                table: "Staffs",
                keyColumn: "StaffId",
                keyValue: 1,
                column: "JoinDate",
                value: new DateTime(2024, 4, 5, 1, 14, 21, 907, DateTimeKind.Local).AddTicks(4475));

            migrationBuilder.UpdateData(
                table: "Staffs",
                keyColumn: "StaffId",
                keyValue: 2,
                column: "JoinDate",
                value: new DateTime(2025, 4, 5, 1, 14, 21, 909, DateTimeKind.Local).AddTicks(1778));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailedAttemptCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");

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
    }
}
