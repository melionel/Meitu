using System;
using System.Collections.Generic;

namespace meitu
{
    class Program
    {
        static User case1 = new User(
            "a",
            UserKind.Black,
            "aaa",
            new List<Goods>
            {
                Utils.PredefinedGoodsList[6]
            }, new List<Coupon>
            {
                Utils.PredefinedCouponsList[0],
                Utils.PredefinedCouponsList[1],
                Utils.PredefinedCouponsList[2],
                Utils.PredefinedCouponsList[3],
                Utils.PredefinedCouponsList[4],
                Utils.PredefinedCouponsList[5],
                Utils.PredefinedCouponsList[6],
                Utils.PredefinedCouponsList[7],
                Utils.PredefinedCouponsList[8]
            });

        private static User case2 = new User(
            "a",
            UserKind.Black,
            "aaa",
            new List<Goods>
            {
                Utils.PredefinedGoodsList[2]
            }, new List<Coupon>
            {
                Utils.PredefinedCouponsList[0],
                Utils.PredefinedCouponsList[1],
                Utils.PredefinedCouponsList[2],
                Utils.PredefinedCouponsList[3],
                Utils.PredefinedCouponsList[4],
                Utils.PredefinedCouponsList[5],
                Utils.PredefinedCouponsList[6],
                Utils.PredefinedCouponsList[7],
                Utils.PredefinedCouponsList[8]
            });

        private static User case3 = new User(
            "a",
            UserKind.Black,
            "aaa",
            new List<Goods>
            {
                Utils.PredefinedGoodsList[0],
                Utils.PredefinedGoodsList[1],
                Utils.PredefinedGoodsList[8],
                Utils.PredefinedGoodsList[11]
            }, new List<Coupon>
            {
                Utils.PredefinedCouponsList[0],
                Utils.PredefinedCouponsList[1],
                Utils.PredefinedCouponsList[2],
                Utils.PredefinedCouponsList[3],
                Utils.PredefinedCouponsList[4],
                Utils.PredefinedCouponsList[5],
                Utils.PredefinedCouponsList[6],
                Utils.PredefinedCouponsList[7],
                Utils.PredefinedCouponsList[8]
            });

        private static User case4 = new User(
            "a",
            UserKind.Black,
            "aaa",
            new List<Goods>
            {
                Utils.PredefinedGoodsList[15],
                Utils.PredefinedGoodsList[13],
                Utils.PredefinedGoodsList[14]
            }, new List<Coupon>
            {
                Utils.PredefinedCouponsList[0],
                Utils.PredefinedCouponsList[1],
                Utils.PredefinedCouponsList[2],
                Utils.PredefinedCouponsList[3],
                Utils.PredefinedCouponsList[4],
                Utils.PredefinedCouponsList[5],
                Utils.PredefinedCouponsList[6],
                Utils.PredefinedCouponsList[7],
                Utils.PredefinedCouponsList[8]
            });

        static void Main(string[] args)
        {
            var finalOrders = Algorithm.GetBestOrderPlan(case4, true, new DateTime(2018, 5, 13));
            var meituOrders = Algorithm.GetMeituOriginialOrderPlan(case4, new DateTime(2018, 5, 13));

            var totalDiscount = 0.0;
            var totalPrice = 0.0;
            foreach (var order in finalOrders)
            {
                Utils.PrintOneOrder(order);
                Console.WriteLine();

                totalPrice += order.GetTotalPrice() + order.GetTotalTax();
                totalDiscount += order.GetTotalDiscount();
            }

            Console.WriteLine(string.Format("Final discount rate:{0:0.000}", totalDiscount/totalPrice));
            Console.WriteLine("-------------------------------------------------------------------");
            Utils.PrintOneOrder(meituOrders);

            Console.Read();
        }
    }
}
