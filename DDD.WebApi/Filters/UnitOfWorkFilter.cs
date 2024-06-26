﻿using DDD.WebApi.Attributes;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DDD.WebApi.Filters
{
    public class UnitOfWorkFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
             // 只有Action执行成功，才自动调用SaveChange
            var result = await next();
            if (result.Exception != null)
            {
                return;
            }
            var actionDes = context.ActionDescriptor as ControllerActionDescriptor;
            if (actionDes == null)
            {
                return;
            }
            var uowAttr = actionDes.MethodInfo.GetCustomAttributes<UnitOfWorkAttribute>();
            if (uowAttr == null)
            {
                return;
            }

            // 遍历UnitOfWorkAttribute中的DbContextTypes，执行特定的操作，例如保存更改
            foreach (var attr in uowAttr)
            {
                foreach (var dbCtxType in attr.DbContextTypes)
                {
                    // 根据 DbContextType 执行特定的操作，例如保存更改
                    if (dbCtxType != null)
                    {
                        // 管 Di 要DbContext实例
                        var dbContext = context.HttpContext.RequestServices.GetService(dbCtxType) as DbContext;
                        if (dbContext != null)
                        {
                            await dbContext.SaveChangesAsync();
                        }
                    }
                }
            }
        }
    }
}