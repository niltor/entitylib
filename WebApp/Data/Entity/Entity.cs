using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Data.Entity
{
    public class Entity : BaseDB
    {
        /// <summary>
        /// 实体名称
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(500)]
        public string Description { get; set; }
        /// <summary>
        /// 实体定义内容
        /// </summary>
        [MaxLength(4000)]
        public string Content { get; set; }
        /// <summary>
        /// 所属类库
        /// </summary>
        public Lib Lib { get; set; }

    }
}
