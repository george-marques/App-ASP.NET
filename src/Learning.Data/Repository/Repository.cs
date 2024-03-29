﻿using Learning.Business.Interfaces;
using Learning.Business.Models;
using Learning.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Learning.Data.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : Entity, new()
    {
        protected readonly AppDbContext Db;
        protected readonly DbSet<T> DbSet;

        protected Repository(AppDbContext db)
        {
            Db = db;
            DbSet = db.Set<T>();
        }

        public virtual async Task Adicionar(T entity)
        {
            DbSet.Add(entity);
            await SaveChanges();
        }

        public virtual async Task Atualizar(T entity)
        {
            var existingEntity = await DbSet.FindAsync(entity.Id); // Supondo que o Id seja a chave primária da entidade
            if (existingEntity != null)
            {
                Db.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            else
            {
                DbSet.Update(entity);
            }

            await SaveChanges();
        }

        public virtual async Task Remover(Guid id)
        {
            DbSet.Remove(new T { Id = id });
            await SaveChanges();
        }

        public async Task<IEnumerable<T>> Buscar(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task<T> ObterPorId(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<List<T>> ObterTodos()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<int> SaveChanges()
        {
            return await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Db?.Dispose();
        }
    }
}
