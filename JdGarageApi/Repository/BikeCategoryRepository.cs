using JdGarageApi.Data;
using JdGarageApi.Models;
using JdGarageApi.Repository.IRepository;

namespace JdGarageApi.Repository
{
    public class BikeCategoryRepository : IBikeCategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public BikeCategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreateCategory(BikeCategory BikeCategory)
        {
            BikeCategory.CreatedDate = DateTime.Now;
            _db.BikeCategory.Add(BikeCategory);
            return Save();
        }

        public bool DeleteCategory(BikeCategory BikeCategory)
        {
            _db.BikeCategory.Remove(BikeCategory);
            return Save();
        }

        public bool ExistCategory(int BikeCategoryId)
        {
            return _db.BikeCategory.Any(x => x.Id == BikeCategoryId);
        }

        public bool ExistCategory(string CategoryName)
        {
            bool name = _db.BikeCategory.Any(x => x.CategoryName.ToLower().Trim() == CategoryName.ToLower().Trim());
            return name;
        }

        public ICollection<BikeCategory> GetCategories()
        {
            return _db.BikeCategory.OrderBy(x => x.CategoryName).ToList();
        }

        public BikeCategory GetCategory(int BikeCategoryId)
        {
            return _db.BikeCategory.FirstOrDefault (x => x.Id == BikeCategoryId);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateCategory(BikeCategory BikeCategory)
        {
            BikeCategory.CreatedDate = DateTime.Now;

            var bikeCategoryExist = _db.BikeCategory.Find(BikeCategory.Id);
            if (bikeCategoryExist != null)
            {
                _db.Entry(bikeCategoryExist).CurrentValues.SetValues(BikeCategory);
            }
            else
            {
                _db.BikeCategory.Update(BikeCategory);
            }
            return Save();
        }
    }
}
