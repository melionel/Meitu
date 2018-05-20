using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace meitu
{
    public class Algorithm
    {

        public static Order GetMeituOriginialOrderPlan(User user, DateTime date)
        {
            var result = new Order();
            var goods = user.GoodsList;
            var coupons = user.CouponList.OrderByDescending(c => c.Discount).ThenByDescending(c => c.MinPrice).ToList();

            foreach (var c in coupons)
            {
                var availableGoods = goods.Where(g => c.IsStatisfiedGoods(g, date));
                if (availableGoods.Sum(g => g.Price) > c.MinPrice)
                {
                    result.GoodsList.AddRange(goods);
                    result.CouponList.Add(c);
                    return result;
                }
            }
            result.GoodsList.AddRange(goods);
            return result;
        }

        static public List<Order> GetBestOrderPlan(User user, bool recommended, DateTime date)
        {
            var result = new List<Order>();

            var leftCoupons =
                GetAllAvailableCoupons(user, user.GoodsList, user.CouponList, date)
                    .OrderByDescending(c => c.Discount)
                    .ThenByDescending(c => c.MinPrice)
                    .ToList();
            var leftGoods = new List<Goods>(user.GoodsList);

            while (leftGoods.Count > 0)
            {
                AddOneBestOrder(user, ref leftGoods, ref leftCoupons, date, true, ref result);
                // update avaliable left coupons
                leftCoupons =
                    GetAllAvailableCoupons(user, leftGoods, leftCoupons, date)
                        .OrderByDescending(c => c.Discount)
                        .ThenBy(c => c.MinPrice)
                        .ToList();
            }

            return result;
        }

        static private List<Coupon> GetAllAvailableCoupons(User user, List<Goods> curGoods, List<Coupon> curCoupons, DateTime date)
        {
            var availableCoupons = new List<Coupon>();
            foreach (var c in curCoupons)
            {
                var availableGoods = curGoods.Where(g => c.IsStatisfiedGoods(g, date));
                if (availableGoods.Sum(g => g.Price) > 0.8 * c.MinPrice)
                {
                    availableCoupons.Add(c);
                }
            }
            return availableCoupons;
        }

        static private void AddOneBestOrder(User user, ref List<Goods> leftGoods, ref List<Coupon> leftCoupons, DateTime date, bool recommended, ref List<Order> finalOrders)
        {
            var order = new Order();
            if (leftCoupons.Count == 0)
            {
                // no left coupons, add all goods to the order
                foreach (var g in leftGoods)
                {
                    order.GoodsList.Add(g);
                }

                finalOrders.Add(order);
                leftGoods.Clear();
            }
            else
            {
                var curCoupon = leftCoupons.First();

                // get all available goods for this coupon and order by the price descending
                var availableGoods = leftGoods.Where(g => curCoupon.IsStatisfiedGoods(g, date)).OrderByDescending(g => g.Price).ToList();
                // iterate on all goods to get enough goods to use this coupon
                var totalPrice = 0.0;
                var recommendedGoods = new List<Goods>();
                foreach (var g in availableGoods)
                {
                    order.GoodsList.Add(g);
                    totalPrice += g.Price;
                    if (totalPrice >= curCoupon.MinPrice)
                    {
                        // we have selected enough goods to use cur coupon
                        break;
                    }
                }

                // no enough, we can recommended goods for user
                if (totalPrice < curCoupon.MinPrice && recommended && curCoupon.RecommondGoodsList.Count > 0)
                {
                    // currently we only recommend one goods
                    var recGoods =
                        curCoupon.RecommondGoodsList.Where(g => g.Price >= (curCoupon.MinPrice - totalPrice))
                            .OrderBy(g => g.Price)
                            .FirstOrDefault();
                    if (recGoods != null)
                    {
                        recommendedGoods.Add(recGoods);
                        totalPrice += recGoods.Price;
                    }
                }

                // if enough goods have been selected, generate a order using this coupon; otherwise abandon this coupon
                if (totalPrice >= curCoupon.MinPrice)
                {
                    foreach (var g in order.GoodsList)
                    {
                        leftGoods.Remove(g);
                    }
                    leftCoupons.Remove(curCoupon);

                    foreach (var rg in recommendedGoods)
                    {
                        order.RecomendedGoodsList.Add(rg);
                    }

                    order.CouponList.Add(curCoupon);
                    finalOrders.Add(order);
                }
                else
                {
                    leftCoupons.Remove(curCoupon);
                }
            }
        }
    }
}
