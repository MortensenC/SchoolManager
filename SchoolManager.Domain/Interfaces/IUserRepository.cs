// -----------------------------------------------------------------------
// <copyright file="IUserRepository.cs" company="Jooin.us">
//
// </copyright>
// -----------------------------------------------------------------------

using SchoolManager.Domain.Entities;

namespace SchoolManager.Domain.Interfaces
{
    using System.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IUserRepository
    {
        IQueryable<User> GetUsers { get; }
    }
}