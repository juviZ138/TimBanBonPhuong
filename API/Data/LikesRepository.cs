using System;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class LikesRepository(AppDbContext context) : ILikesRepository
{
    public void AddLike(MemberLike like)
    {
        context.Likes.Add(like);
    }

    public void DeleteLike(MemberLike like)
    {
        context.Likes.Remove(like);
    }

    public async Task<IReadOnlyList<string>> GetCurrentMemberLikeIds(string memberId)
    {
        return await context.Likes
            .Where(x => x.SourceMemberId == memberId)
            .Select(x => x.TargetMemberId)
            .ToListAsync();
    }

    public async Task<MemberLike?> GetMemberLike(string sourceMemberId, string targetMemberId)
    {
        // return await context.Likes.SingleOrDefaultAsync
        //     (x => x.SourceMemberId == sourceMemberId && x.TargetMemberId == targetMemberId);

        return await context.Likes.FindAsync(sourceMemberId, targetMemberId);
    }

    public async Task<PaginatedResult<Member>> GetMemberLikes(LikesParams likesParams)
    {
        var query = context.Likes.AsQueryable();
        IQueryable<Member> result;

        switch (likesParams.Predicate)
        {
            case "liked":
                result = query
                        .Where(x => x.SourceMemberId == likesParams.MemberId)
                        .Select(x => x.TargetMember);
                break;

            case "likedBy":
                result = query
                        .Where(x => x.TargetMemberId == likesParams.MemberId)
                        .Select(x => x.SourceMember);
                break;

            default: // mutual
                // get who i like
                var likeIds = await GetCurrentMemberLikeIds(likesParams.MemberId);
                // get who like me
                result = query
                        .Where(x => x.TargetMemberId == likesParams.MemberId
                            && likeIds.Contains(x.SourceMemberId)) // check thier id in my like id
                        .Select(x => x.SourceMember);
                break;
        }

        return await PaginationHelper.CreateAsync(result, likesParams.PageNumber, likesParams.PageSize);
    }


}
