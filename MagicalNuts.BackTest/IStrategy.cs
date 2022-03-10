using MagicalNuts.Primitive;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagicalNuts.BackTest
{
	/// <summary>
	/// 売買戦略のインターフェースを表します。
	/// </summary>
	public interface IStrategy : IPropertyHolder
	{
		/// <summary>
		/// 非同期で準備します。
		/// </summary>
		/// <returns>非同期タスク</returns>
		Task SetUpAsync();

		/// <summary>
		/// 注文を取得します。
		/// </summary>
		/// <param name="state">バックテストの状態</param>
		/// <param name="orders">注文のリスト</param>
		void GetOrders(BackTestStatus state, List<Order> orders);
	}
}
