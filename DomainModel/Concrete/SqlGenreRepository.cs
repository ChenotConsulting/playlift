using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel.Abstract;
using DomainModel.Entities;

namespace DomainModel.Concrete
{
    public class SqlGenreRepository : IGenreRepository
    {
        private readonly Table<Genre> genreTable;
        public SqlGenreRepository(string connString)
        {
            genreTable = (new DataContext(connString)).GetTable<Genre>();
        }

        public IQueryable<Genre> Genre { get { return genreTable.OrderBy(x => x.GenreName); } }
        public Genre GetGenreById(int genreId)
        {
            return genreTable.FirstOrDefault(x => x.GenreId == genreId);
        }
    }
}
