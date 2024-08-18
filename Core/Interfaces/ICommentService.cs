﻿using Core.PassengerContext.Booking;
using Core.PassengerContext.Booking.Enums;

namespace Core.Interfaces
{
    public interface ICommentService
    {
        /// <summary>
        /// Adds a comment asynchronously.
        /// </summary>
        /// <param name="id">The ID of the passenger or item.</param>
        /// <param name="commentType">The type of the comment.</param>
        /// <param name="text">The text of the comment.</param>
        /// <param name="flightIds">The list of flight IDs associated with the comment.</param>
        /// <param name="predefinedCommentId">The ID of the predefined comment (optional).</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation. The task
        /// result contains the added comment.</returns>
        Task<Comment> AddCommentAsync(Guid id, CommentTypeEnum commentType, string text, List<Guid> flightIds,
            string predefinedCommentId = null);
    }
}