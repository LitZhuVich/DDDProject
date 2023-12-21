using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.Results
{
    public enum CheckCodeResult
    {
        /// <summary>
        /// 返回成功
        /// </summary>
        Ok,
        /// <summary>
        /// 手机号不存在
        /// </summary>
        PhoneNumberNotFound,
        /// <summary>
        /// 用户被锁定
        /// </summary>
        Lockout,
        /// <summary>
        /// 验证码错误
        /// </summary>
        CodeError
    }
}
