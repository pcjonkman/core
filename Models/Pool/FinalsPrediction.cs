using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.Pool
{
  public class FinalsPrediction : Entity {
    // [Required, ForeignKey("PoolPlayerId")]
    // public virtual PoolPlayer PoolPlayer { get; set; }

    [Required]
    public int PoolPlayerId { get; set; }

    // [Required, ForeignKey("FinalsId")]
    // public virtual Finals Finals { get; set; }

    [Required]
    public int FinalsId { get; set; }

    // [Required, ForeignKey("CountryId")]
    // public virtual Country Country { get; set; }

    [Required]
    public int CountryId { get; set; }

    [Required]
    public int SubScore { get; set; }

    /* Defaults property cannot via Annotations, only through FluentApi
    public class CoreContext : DbContext
    {
        public DbSet<FinalsPrediction> FinalsPrediction { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FinalsPrediction>()
                .Property(b => b.Subscore)
                .HasDefaultValue(0);
        }
    }
     */

  }
}

/*
CREATE TABLE [dbo].[FinalsPrediction](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PoolPlayer] [int] NOT NULL,
	[Final] [int] NOT NULL,
	[Country] [int] NOT NULL,
	[SubScore] [int] NOT NULL,
 CONSTRAINT [PK_FinalsPrediction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 */

/*
ALTER TABLE [dbo].[FinalsPrediction] ADD  CONSTRAINT [DF_FinalsPrediction_SubScore]  DEFAULT ((0)) FOR [SubScore]
ALTER TABLE [dbo].[FinalsPrediction]  WITH CHECK ADD  CONSTRAINT [FK_FinalsPrediction_Country] FOREIGN KEY([Country]) REFERENCES [dbo].[Country] ([ID])
ALTER TABLE [dbo].[FinalsPrediction]  WITH CHECK ADD  CONSTRAINT [FK_FinalsPrediction_Finals] FOREIGN KEY([Final]) REFERENCES [dbo].[Finals] ([ID])
ALTER TABLE [dbo].[FinalsPrediction]  WITH CHECK ADD  CONSTRAINT [FK_FinalsPrediction_PoolPlayer] FOREIGN KEY([PoolPlayer]) REFERENCES [dbo].[PoolPlayer] ([ID])
  */