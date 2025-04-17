using System;
using Microsoft.EntityFrameworkCore;

namespace EducationCenterAPI.Dtos;

public class PagedList<T>
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public List<T> Data { get; set; } = new List<T>();
    public bool HasNext => CurrentPage * PageSize < TotalCount;
    public bool HasPrevious => CurrentPage > 1;

    private PagedList(int totalCount, int pageSize, int currentPage, List<T> data)
    {
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
        Data = data;
    }
    public static async Task<PagedList<T>> Create(IQueryable<T> query, int currentPage, int pageSize)
    {
        if (currentPage < 1) currentPage = 1;
        if (pageSize < 1) pageSize = 10;
        var tc = await query.CountAsync();
        var data = await query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PagedList<T>(tc, pageSize, currentPage, data);
    }

}
