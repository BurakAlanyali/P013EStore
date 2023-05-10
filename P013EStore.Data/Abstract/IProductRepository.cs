using P013EStore.Core.Entities;
using System.Linq.Expressions;

namespace P013EStore.Data.Abstract
{
	public interface IProductRepository : IRepository<Product> 
	{
		Task<Product> GetProductByIncludeAsync(int id); // bu metot bize ürüne marka ve kategori include edilmiş veritabanından bir tane kayıt getirecek
		Task<List<Product>> GetProductsByIncludeAsync();// tüm ürünleri marka ve kategorisyle getirecek metot 
		Task<List<Product>> GetProductsByIncludeAsync(Expression<Func<Product, bool>> expression);// Belli kurala göre bir grup ürünleri marka ve kategori bilgileriyle getirecek metot
	}
}
