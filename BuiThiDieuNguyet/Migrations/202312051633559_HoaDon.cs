namespace BuiThiDieuNguyet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HoaDon : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChiTietHoaDons",
                c => new
                    {
                        MaChiTietHoaDon = c.Int(nullable: false, identity: true),
                        MaHoaDon = c.Int(nullable: false),
                        MaMovie = c.Int(nullable: false),
                        SoLuong = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MaChiTietHoaDon)
                .ForeignKey("dbo.HoaDons", t => t.MaHoaDon, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.MaMovie, cascadeDelete: true)
                .Index(t => t.MaHoaDon)
                .Index(t => t.MaMovie);
            
            CreateTable(
                "dbo.HoaDons",
                c => new
                    {
                        MaHoaDon = c.Int(nullable: false, identity: true),
                        TongTien = c.Double(nullable: false),
                        NgayLap = c.DateTime(nullable: false),
                        MaKhachHang = c.Int(),
                    })
                .PrimaryKey(t => t.MaHoaDon);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChiTietHoaDons", "MaMovie", "dbo.Movies");
            DropForeignKey("dbo.ChiTietHoaDons", "MaHoaDon", "dbo.HoaDons");
            DropIndex("dbo.ChiTietHoaDons", new[] { "MaMovie" });
            DropIndex("dbo.ChiTietHoaDons", new[] { "MaHoaDon" });
            DropTable("dbo.HoaDons");
            DropTable("dbo.ChiTietHoaDons");
        }
    }
}
