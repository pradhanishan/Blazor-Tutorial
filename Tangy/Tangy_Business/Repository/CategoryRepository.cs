using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_Models;

namespace Tangy_Business.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public CategoryRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<CategoryDTO> Create(CategoryDTO objDTO)
        {
            Category obj = _mapper.Map<CategoryDTO, Category>(objDTO);

            await _db.Categories.AddAsync(obj);
            _db.SaveChangesAsync();

            return _mapper.Map<Category, CategoryDTO>(obj);

        }



        public async Task<int> Delete(int id)
        {
            Category obj = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (obj != null)
            {
                _db.Categories.Remove(obj);
                return await _db.SaveChangesAsync();
            }

            return 0;

        }

        public async Task<CategoryDTO> Get(int id)
        {
            Category obj = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (obj != null)
            {
                return _mapper.Map<Category, CategoryDTO>(obj);

            }
            return new CategoryDTO();
        }

        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            return _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(_db.Categories);
        }

        public async Task<CategoryDTO> Update(CategoryDTO objDTO)
        {
            Category objFromDb = await _db.Categories.FirstOrDefaultAsync(x => x.Id == objDTO.Id);

            if (objFromDb != null)
            {
                objFromDb.Name = objDTO.Name;
                _db.Categories.Update(objFromDb);
                await _db.SaveChangesAsync();
                return _mapper.Map<Category, CategoryDTO>(objFromDb);
            }

            return new CategoryDTO();
        }
    }
}
