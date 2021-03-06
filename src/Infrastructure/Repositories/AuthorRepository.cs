﻿using Core.Repositories;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository<IdentityUser>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public AuthorRepository(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public async Task<IEnumerable<IdentityUser>> GetAuthorsAsync(CancellationToken cancellationToken)
        {
            var authors = await _dbContext.Users
                                        .ToListAsync(cancellationToken);

            return authors;
        }
        public async Task<IdentityUser> DeleteAuthorAsync(string id, CancellationToken cancellationToken)
        {
            var author = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id==id, cancellationToken);
            await _userManager.DeleteAsync(author);

            return author;
        }

        public async Task<int> CountAuthorsAsync(CancellationToken cancellationToken = default)
        {
            var authors = await _dbContext.Users
                                       .ToListAsync(cancellationToken);
            return authors.Count;
        }
        public async Task<IEnumerable<Article>> GetArticlesByUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            var articles = await _dbContext.Articles
                         .Include(a => a.Author)
                         .Where(a => a.Author.Id == userId)
                         .ToListAsync(cancellationToken);

            return articles;
        }
    }
}
