using CalculadoraRest.Model.Base;
using CalculadoraRest.Model.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculadoraRest.Repository.Generic
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected MySqlContext _context;
        private DbSet<T> _dataset;
        public GenericRepository(MySqlContext context)
        {
            _context = context;
            _dataset = _context.Set<T>();
        }

        public T Create(T item)
        {
            try
            {
                _dataset.Add(item);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return item;
        }

        public void Delete(long id)
        {
            var result = _dataset.SingleOrDefault(_ => _.Id.Equals(id));
            if (result != null)
            {
                try
                {
                    _dataset.Add(result);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public bool Exists(long id)
        {
            return _dataset.Any(_ => _.Id.Equals(id));
        }

        public List<T> FindAll()
        {
            return _dataset.ToList();
        }

        public T FindByID(long id)
        {
            return _dataset.Find(id);
        }

        public T Update(T item)
        {
            if (!Exists(item.Id)) return null;

            var result = _context.Persons.SingleOrDefault(_ => _.Id.Equals(item.Id));

            if (result != null)
            {
                try
                {
                    _context.Entry(item).CurrentValues.SetValues(item);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
            {
                return null;
            }
            return item;
        }
    }
}
