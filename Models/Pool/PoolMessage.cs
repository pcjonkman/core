using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.Pool
{
  public class PoolMessage : Entity {

    // [Required, ForeignKey("PoolPlayerId")]
    // public virtual PoolPlayer PoolPlayer { get; set; }

    [Required]
    public int PoolPlayerId { get; set; }

    [Required, Column(TypeName = "DateTime")]
    public DateTime PlacedDate { get; set; }

    [MaxLength(255), RegularExpression("^[^<>]+$")]
    public string Message { get; set; }

    public MessageStatus Status { get; set; }

    public enum MessageStatus
    {
        Submitted,
        Approved,
        Rejected
    }

  }
}

/*
CREATE TABLE [dbo].[PoolMessage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PoolPlayer] [int] NOT NULL,
	[PlacedDate] [datetime] NOT NULL,
	[Message] [nvarchar](250) NULL,
 CONSTRAINT [PK_PoolMessage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 */

/*
ALTER TABLE [dbo].[PoolMessage]  WITH CHECK ADD  CONSTRAINT [FK_PoolMessage_PoolPlayer] FOREIGN KEY([PoolPlayer]) REFERENCES [dbo].[PoolPlayer] ([ID])
  */