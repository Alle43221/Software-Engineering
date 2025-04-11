using System;
using System.Collections.Generic;
using BugTracker.Domain;

namespace BugTracker.Repository
{
    public interface IRepository<ID, E> where E : Entity<ID>
    {
        /// <summary>
        /// Finds an entity with the given id.
        /// </summary>
        /// <param name="id">The id of the entity to be returned. Must not be null.</param>
        /// <returns>The entity with the specified id or null if there is no entity with the given id.</returns>
        /// <exception cref="ArgumentException">Thrown if id is null.</exception>
        E? FindOne(ID id);

        /// <summary>
        /// Returns an enumerable collection of all entities.
        /// </summary>
        /// <returns>All entities.</returns>
        IEnumerable<E> FindAll();

        /// <summary>
        /// Saves a given entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to save. Must not be null.</param>
        /// <returns>Null if the entity is saved successfully; otherwise, returns the existing entity (if id already exists).</returns>
        /// <exception cref="ArgumentException">Thrown if the given entity is null.</exception>
        E? Save(E entity);

        /// <summary>
        /// Removes the entity with the specified id.
        /// </summary>
        /// <param name="id">The id of the entity to be removed. Must not be null.</param>
        /// <returns>The removed entity or null if no entity exists with the given id.</returns>
        /// <exception cref="ArgumentException">Thrown if the given id is null.</exception>
        E? Delete(ID id);

        /// <summary>
        /// Updates an existing entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to update. Must not be null.</param>
        /// <returns>Null if the entity is updated successfully; otherwise, returns the existing entity (if id does not exist).</returns>
        /// <exception cref="ArgumentException">Thrown if the given entity is null.</exception>
        E? Update(E entity);
    }
}