using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.Pool
{
  public class Finals : Entity {
    [Required, MaxLength(50), RegularExpression("^[ -a-zA-Z0-9]*$")]
    public string LevelName { get; set; }

    [Range(0, 10)]
    public int LevelNumber { get; set; }

		/* Unique property cannot via Annotations, only through FluentApi
			public class CoreContext : DbContext
			{
					public DbSet<Finals> Finals { get; set; }
					
					protected override void OnModelCreating(ModelBuilder modelBuilder)
					{
							modelBuilder.Entity<Finals>()
									.HasAlternateKey(a => a.LevelNumber);
					}
			}
		 */
  }
}

/*
CREATE TABLE [dbo].[Finals](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LevelName] [nvarchar](50) NOT NULL,
	[LevelNumber] [int] NULL,
 CONSTRAINT [PK_Finals] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 */

/*
ALTER TABLE [dbo].[Finals] ADD CONSTRAINT  [UC_Finals_LevelNumber] UNIQUE ([LevelNumber])
  */