using Microsoft.EntityFrameworkCore;
using P013EStore.Core.Entities;
using P013EStore.Data.Abstract;
using System.Linq.Expressions;

namespace P013EStore.Data.Concrete
{
    public class Repository<T> : IRepository<T> where T : class, IEntity, new() // Buraya gelecek T lerin şartlarını belirledik: T(brand,product,category vb) bir class olmalı, IEntity den implemente almalı ve new lenebilir olmalı.
    {
        internal DatabaseContext _context;// boş bir database context oluşturduk
        internal DbSet<T> _dbSet; // boş bir dbset tanımladık. Repository e gönderilecek T classını parametre verdik.
        public Repository(DatabaseContext context) // S.O.L.I.D prensipleri
        {
            _context = context; // context i burada doldurduk, aşağıda kullanabilmek için(doldurmadan kullanmak istersek null reference hatası olur.)
            _dbSet = context.Set<T>();// repository e gönderilecek T class ı için context üzerindeki db sete göre kendini ayarla
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public T Find(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T> FindAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            return _dbSet.FirstOrDefault(expression);
        }

        public List<T> GetAll()
        {
            return _dbSet.AsNoTracking().ToList();//performans için asnotracking koyduk
        }

        public List<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return _dbSet.AsNoTracking().Where(expression).ToList();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AsNoTracking().Where(expression).ToListAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }

        public int Save()
        {
            return _context.SaveChanges(); // entity framework de SaveChanges metodu(ekle,güncelle,sil vb. işlemleri db ye işleme metodu) direk context üzerinden çalışır, dbset in böyle bir metodu olmadığı için _context.SaveChanges() diyerek database context imiz üzerindeki bütün crud işlemlerini veritabanına yansıtmamızı sağlayan bu metodu 1 kere çağırmamız gerekli yoksa işlemler db ye işlenmez!!
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
