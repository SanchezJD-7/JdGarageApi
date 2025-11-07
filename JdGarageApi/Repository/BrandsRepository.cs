using JdGarageApi.Data;
using JdGarageApi.Models;
using JdGarageApi.Repository.IRepository;

namespace JdGarageApi.Repository
{
    public class BrandsRepository : IBrandRepository
    {
        private readonly ApplicationDbContext _db;

        public BrandsRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreateBrand(Brands brand)
        {
            brand.CreationDate = DateTime.Now;
            brand.LastUpdateDate = null;
            _db.Brands.Add(brand);
            return Save();
        }

        public bool DeleteBrand(Brands brand)
        {
            _db.Brands.Remove(brand);
            return Save();
        }

        public bool ExistBrand(string brandName, string brandType)
        {
            return _db.Brands.Any(x => x.BrandName.ToLower() == brandName.ToLower() && x.BrandType.ToLower() == brandType.ToLower());
        }

        public ICollection<Brands> GetBrands(string brandType)
        {
            return _db.Brands
                .Where(x => x.BrandType.ToLower() == brandType.ToLower())
                .OrderBy(x => x.BrandName)
                .ToList();
        }

        public int GetTotalBrands(string brandType)
        {
            var totalBrands = _db.Brands.AsQueryable();
            totalBrands = totalBrands.Where(x => x.BrandType.ToLower() == brandType.ToLower());
            return totalBrands.Count();
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0;
        }

        public bool UpdateBrand(Brands brand)
        {
            var brandExist = _db.Brands.Find(brand.Id);
            if (brandExist != null)
            {
                var creationDate = brandExist.CreationDate;
                _db.Entry(brandExist).CurrentValues.SetValues(brand);

                brandExist.CreationDate = creationDate;
                brandExist.LastUpdateDate = DateTime.Now;
            }
            else
            {
                brand.LastUpdateDate = DateTime.Now;
                _db.Brands.Update(brand);
            }

            return Save();
        }

        public Brands GetBrand(int brandId)
        {
            return _db.Brands.FirstOrDefault(x => x.Id == brandId);
        }

    }
}
