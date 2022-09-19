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
    public class ProductPriceRepository : IProductPriceRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ProductPriceRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<ProductPriceDTO> Create(ProductPriceDTO objDTO)
        {
            ProductPrice obj = _mapper.Map<ProductPriceDTO, ProductPrice>(objDTO);

            await _db.ProductPrices.AddAsync(obj);
            await _db.SaveChangesAsync();

            return _mapper.Map<ProductPrice, ProductPriceDTO>(obj);

        }



        public async Task<int> Delete(int id)
        {
            ProductPrice obj = await _db.ProductPrices.FirstOrDefaultAsync(x => x.Id == id);

            if (obj != null)
            {
                _db.ProductPrices.Remove(obj);
                return await _db.SaveChangesAsync();
            }

            return 0;

        }

        public async Task<ProductPriceDTO> Get(int id)
        {
            ProductPrice obj = await _db.ProductPrices.Include(x=>x.Product).FirstOrDefaultAsync(x => x.Id == id);

            if (obj != null)
            {
                return _mapper.Map<ProductPrice, ProductPriceDTO>(obj);

            }
            return new ProductPriceDTO();
        }

        public async Task<IEnumerable<ProductPriceDTO>> GetAll(int? id = null)
        {
            if(id!=null && id > 0)
            {
                return _mapper.Map<IEnumerable<ProductPrice>, IEnumerable<ProductPriceDTO>>(_db.ProductPrices.Include(x => x.Product));
            }
            else
            {
                return _mapper.Map<IEnumerable<ProductPrice>, IEnumerable<ProductPriceDTO>>(_db.ProductPrices.Where(x => x.ProductId == id));
            }
            
        }

        public async Task<ProductPriceDTO> Update(ProductPriceDTO objDTO)
        {
            ProductPrice objFromDb = await _db.ProductPrices.FirstOrDefaultAsync(x => x.Id == objDTO.Id);

            if (objFromDb != null)
            {
                objFromDb.Price = objDTO.Price;
                objFromDb.Size = objDTO.Size;
                _db.ProductPrices.Update(objFromDb);
                await _db.SaveChangesAsync();
                return _mapper.Map<ProductPrice, ProductPriceDTO>(objFromDb);
            }

            return new ProductPriceDTO();
        }
    }
}
