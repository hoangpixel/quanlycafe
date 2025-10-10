-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Sep 19, 2025 at 09:12 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `quancafe`
--

-- --------------------------------------------------------

--
-- Table structure for table `ban`
--

CREATE TABLE `ban` (
  `MABAN` bigint(20) NOT NULL,
  `TENBAN` varchar(64) NOT NULL,
  `DANGSUDUNG` tinyint(1) NOT NULL DEFAULT 0,
  `MADONHIENTAI` bigint(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `bienthe`
--

CREATE TABLE `bienthe` (
  `MABIENTHE` bigint(20) NOT NULL,
  `MASANPHAM` bigint(20) NOT NULL,
  `TENBIENTHE` varchar(64) NOT NULL,
  `GIA` decimal(12,2) NOT NULL DEFAULT 0.00,
  `CONHOATDONG` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `congthuc`
--

CREATE TABLE `congthuc` (
  `MACONGTHUC` bigint(20) NOT NULL,
  `MABIENTHE` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `ctcongthuc`
--

CREATE TABLE `ctcongthuc` (
  `MACTCT` bigint(20) NOT NULL,
  `MACONGTHUC` bigint(20) NOT NULL,
  `MANGUYENLIEU` bigint(20) NOT NULL,
  `SOLUONGCOSO` decimal(12,3) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `ctdonhang`
--

CREATE TABLE `ctdonhang` (
  `MACTDON` bigint(20) NOT NULL,
  `MADON` bigint(20) NOT NULL,
  `MABIENTHE` bigint(20) NOT NULL,
  `SOLUONG` decimal(12,3) NOT NULL,
  `GIATAITHOIDIEM` decimal(12,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `cthd`
--

CREATE TABLE `cthd` (
  `MACTHD` bigint(20) NOT NULL,
  `MAHOADON` bigint(20) NOT NULL,
  `MABIENTHE` bigint(20) NOT NULL,
  `SOLUONG` decimal(12,3) NOT NULL,
  `DONGIA` decimal(12,2) NOT NULL,
  `THANHTIEN` decimal(12,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `ctphieunhap`
--

CREATE TABLE `ctphieunhap` (
  `MACTPN` bigint(20) NOT NULL,
  `MAPN` bigint(20) NOT NULL,
  `MANGUYENLIEU` bigint(20) NOT NULL,
  `SOLUONGCOSO` decimal(12,3) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `danhmuc`
--

CREATE TABLE `danhmuc` (
  `MADANHMUC` bigint(20) NOT NULL,
  `TENDANHMUC` varchar(64) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `donhang`
--

CREATE TABLE `donhang` (
  `MADON` bigint(20) NOT NULL,
  `MABAN` bigint(20) NOT NULL,
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 0,
  `THOIGIANTAO` datetime NOT NULL DEFAULT current_timestamp(),
  `THOIGIANXACNHAN` datetime DEFAULT NULL,
  `THOIGIAN_THANHTOAN` datetime DEFAULT NULL,
  `TAOBOI` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `hoadon`
--

CREATE TABLE `hoadon` (
  `MAHOADON` bigint(20) NOT NULL,
  `MADON` bigint(20) NOT NULL,
  `DATHANHTOAN` tinyint(1) NOT NULL DEFAULT 0,
  `THOIGIANIN` datetime NOT NULL DEFAULT current_timestamp(),
  `TONGTIEN` decimal(12,2) NOT NULL DEFAULT 0.00
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `nguoidung`
--

CREATE TABLE `nguoidung` (
  `MANGUOIDUNG` bigint(20) NOT NULL,
  `HOTEN` varchar(128) NOT NULL,
  `SODIENTHOAI` varchar(32) DEFAULT NULL,
  `EMAIL` varchar(128) DEFAULT NULL,
  `MATKHAU` varchar(255) NOT NULL,
  `CONHOATDONG` tinyint(1) NOT NULL DEFAULT 1,
  `NGAYTAO` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `nguyenlieu`
--

CREATE TABLE `nguyenlieu` (
  `MANGUYENLIEU` bigint(20) NOT NULL,
  `TENNGUYENLIEU` varchar(128) NOT NULL,
  `DONVICOSO` enum('g','ml','cai') NOT NULL,
  `CONHOATDONG` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `nhacungcap`
--

CREATE TABLE `nhacungcap` (
  `MANCC` bigint(20) NOT NULL,
  `TENNCC` varchar(128) NOT NULL,
  `SODIENTHOAI` varchar(32) DEFAULT NULL,
  `EMAIL` varchar(128) DEFAULT NULL,
  `DIACHI` varchar(255) DEFAULT NULL,
  `CONHOATDONG` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `nhatkykho`
--

CREATE TABLE `nhatkykho` (
  `MANK` bigint(20) NOT NULL,
  `MANGUYENLIEU` bigint(20) NOT NULL,
  `SOLUONG` decimal(12,3) NOT NULL,
  `LOAINGUON` enum('PHIEUNHAP','HOADON','DIEUCHINH','PHAHUY') NOT NULL,
  `MANGUON` bigint(20) NOT NULL,
  `THOIGIAN` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `phieunhap`
--

CREATE TABLE `phieunhap` (
  `MAPN` bigint(20) NOT NULL,
  `MANCC` bigint(20) NOT NULL,
  `THOIGIAN` datetime NOT NULL DEFAULT current_timestamp(),
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `sanpham`
--

CREATE TABLE `sanpham` (
  `MASANPHAM` bigint(20) NOT NULL,
  `MADANHMUC` bigint(20) NOT NULL,
  `TENSANPHAM` varchar(128) NOT NULL,
  `CONHOATDONG` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `thanhtoan`
--

CREATE TABLE `thanhtoan` (
  `MATT` bigint(20) NOT NULL,
  `MAHOADON` bigint(20) NOT NULL,
  `HINHTHUC` enum('TIENMAT','THE','VIDIENTU') NOT NULL,
  `SOTIEN` decimal(12,2) NOT NULL,
  `THOIGIANTT` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `vaitro`
--

CREATE TABLE `vaitro` (
  `MAVAITRO` bigint(20) NOT NULL,
  `TENVAITRO` varchar(64) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `vaitronguoidung`
--

CREATE TABLE `vaitronguoidung` (
  `MANGUOIDUNG` bigint(20) NOT NULL,
  `MAVAITRO` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `ban`
--
ALTER TABLE `ban`
  ADD PRIMARY KEY (`MABAN`),
  ADD UNIQUE KEY `UQ_TENBAN` (`TENBAN`);

--
-- Indexes for table `bienthe`
--
ALTER TABLE `bienthe`
  ADD PRIMARY KEY (`MABIENTHE`),
  ADD KEY `FK_BT_SP` (`MASANPHAM`);

--
-- Indexes for table `congthuc`
--
ALTER TABLE `congthuc`
  ADD PRIMARY KEY (`MACONGTHUC`),
  ADD UNIQUE KEY `MABIENTHE` (`MABIENTHE`);

--
-- Indexes for table `ctcongthuc`
--
ALTER TABLE `ctcongthuc`
  ADD PRIMARY KEY (`MACTCT`),
  ADD KEY `FK_CTCT_CT` (`MACONGTHUC`),
  ADD KEY `FK_CTCT_NL` (`MANGUYENLIEU`);

--
-- Indexes for table `ctdonhang`
--
ALTER TABLE `ctdonhang`
  ADD PRIMARY KEY (`MACTDON`),
  ADD KEY `FK_CTDH_DH` (`MADON`),
  ADD KEY `FK_CTDH_BT` (`MABIENTHE`);

--
-- Indexes for table `cthd`
--
ALTER TABLE `cthd`
  ADD PRIMARY KEY (`MACTHD`),
  ADD KEY `FK_CTHD_HD` (`MAHOADON`),
  ADD KEY `FK_CTHD_BT` (`MABIENTHE`);

--
-- Indexes for table `ctphieunhap`
--
ALTER TABLE `ctphieunhap`
  ADD PRIMARY KEY (`MACTPN`),
  ADD KEY `FK_CTPN_PN` (`MAPN`),
  ADD KEY `FK_CTPN_NL` (`MANGUYENLIEU`);

--
-- Indexes for table `danhmuc`
--
ALTER TABLE `danhmuc`
  ADD PRIMARY KEY (`MADANHMUC`),
  ADD UNIQUE KEY `TENDANHMUC` (`TENDANHMUC`);

--
-- Indexes for table `donhang`
--
ALTER TABLE `donhang`
  ADD PRIMARY KEY (`MADON`),
  ADD KEY `FK_DH_BAN` (`MABAN`),
  ADD KEY `FK_DH_ND` (`TAOBOI`);

--
-- Indexes for table `hoadon`
--
ALTER TABLE `hoadon`
  ADD PRIMARY KEY (`MAHOADON`),
  ADD UNIQUE KEY `MADON` (`MADON`);

--
-- Indexes for table `nguoidung`
--
ALTER TABLE `nguoidung`
  ADD PRIMARY KEY (`MANGUOIDUNG`),
  ADD UNIQUE KEY `EMAIL` (`EMAIL`);

--
-- Indexes for table `nguyenlieu`
--
ALTER TABLE `nguyenlieu`
  ADD PRIMARY KEY (`MANGUYENLIEU`),
  ADD UNIQUE KEY `UQ_TENNL` (`TENNGUYENLIEU`);

--
-- Indexes for table `nhacungcap`
--
ALTER TABLE `nhacungcap`
  ADD PRIMARY KEY (`MANCC`);

--
-- Indexes for table `nhatkykho`
--
ALTER TABLE `nhatkykho`
  ADD PRIMARY KEY (`MANK`),
  ADD KEY `FK_NK_NL` (`MANGUYENLIEU`);

--
-- Indexes for table `phieunhap`
--
ALTER TABLE `phieunhap`
  ADD PRIMARY KEY (`MAPN`),
  ADD KEY `FK_PN_NCC` (`MANCC`);

--
-- Indexes for table `sanpham`
--
ALTER TABLE `sanpham`
  ADD PRIMARY KEY (`MASANPHAM`),
  ADD KEY `FK_SP_DM` (`MADANHMUC`);

--
-- Indexes for table `thanhtoan`
--
ALTER TABLE `thanhtoan`
  ADD PRIMARY KEY (`MATT`),
  ADD KEY `FK_TT_HD` (`MAHOADON`);

--
-- Indexes for table `vaitro`
--
ALTER TABLE `vaitro`
  ADD PRIMARY KEY (`MAVAITRO`),
  ADD UNIQUE KEY `TENVAITRO` (`TENVAITRO`);

--
-- Indexes for table `vaitronguoidung`
--
ALTER TABLE `vaitronguoidung`
  ADD PRIMARY KEY (`MANGUOIDUNG`,`MAVAITRO`),
  ADD KEY `FK_NDVT_VT` (`MAVAITRO`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `ban`
--
ALTER TABLE `ban`
  MODIFY `MABAN` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `bienthe`
--
ALTER TABLE `bienthe`
  MODIFY `MABIENTHE` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `congthuc`
--
ALTER TABLE `congthuc`
  MODIFY `MACONGTHUC` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `ctcongthuc`
--
ALTER TABLE `ctcongthuc`
  MODIFY `MACTCT` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `ctdonhang`
--
ALTER TABLE `ctdonhang`
  MODIFY `MACTDON` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `cthd`
--
ALTER TABLE `cthd`
  MODIFY `MACTHD` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `ctphieunhap`
--
ALTER TABLE `ctphieunhap`
  MODIFY `MACTPN` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `danhmuc`
--
ALTER TABLE `danhmuc`
  MODIFY `MADANHMUC` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `donhang`
--
ALTER TABLE `donhang`
  MODIFY `MADON` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `hoadon`
--
ALTER TABLE `hoadon`
  MODIFY `MAHOADON` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `nguoidung`
--
ALTER TABLE `nguoidung`
  MODIFY `MANGUOIDUNG` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `nguyenlieu`
--
ALTER TABLE `nguyenlieu`
  MODIFY `MANGUYENLIEU` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `nhacungcap`
--
ALTER TABLE `nhacungcap`
  MODIFY `MANCC` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `nhatkykho`
--
ALTER TABLE `nhatkykho`
  MODIFY `MANK` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `phieunhap`
--
ALTER TABLE `phieunhap`
  MODIFY `MAPN` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `sanpham`
--
ALTER TABLE `sanpham`
  MODIFY `MASANPHAM` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `thanhtoan`
--
ALTER TABLE `thanhtoan`
  MODIFY `MATT` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `vaitro`
--
ALTER TABLE `vaitro`
  MODIFY `MAVAITRO` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `bienthe`
--
ALTER TABLE `bienthe`
  ADD CONSTRAINT `FK_BT_SP` FOREIGN KEY (`MASANPHAM`) REFERENCES `sanpham` (`MASANPHAM`);

--
-- Constraints for table `congthuc`
--
ALTER TABLE `congthuc`
  ADD CONSTRAINT `FK_CT_BT` FOREIGN KEY (`MABIENTHE`) REFERENCES `bienthe` (`MABIENTHE`);

--
-- Constraints for table `ctcongthuc`
--
ALTER TABLE `ctcongthuc`
  ADD CONSTRAINT `FK_CTCT_CT` FOREIGN KEY (`MACONGTHUC`) REFERENCES `congthuc` (`MACONGTHUC`),
  ADD CONSTRAINT `FK_CTCT_NL` FOREIGN KEY (`MANGUYENLIEU`) REFERENCES `nguyenlieu` (`MANGUYENLIEU`);

--
-- Constraints for table `ctdonhang`
--
ALTER TABLE `ctdonhang`
  ADD CONSTRAINT `FK_CTDH_BT` FOREIGN KEY (`MABIENTHE`) REFERENCES `bienthe` (`MABIENTHE`),
  ADD CONSTRAINT `FK_CTDH_DH` FOREIGN KEY (`MADON`) REFERENCES `donhang` (`MADON`);

--
-- Constraints for table `cthd`
--
ALTER TABLE `cthd`
  ADD CONSTRAINT `FK_CTHD_BT` FOREIGN KEY (`MABIENTHE`) REFERENCES `bienthe` (`MABIENTHE`),
  ADD CONSTRAINT `FK_CTHD_HD` FOREIGN KEY (`MAHOADON`) REFERENCES `hoadon` (`MAHOADON`);

--
-- Constraints for table `ctphieunhap`
--
ALTER TABLE `ctphieunhap`
  ADD CONSTRAINT `FK_CTPN_NL` FOREIGN KEY (`MANGUYENLIEU`) REFERENCES `nguyenlieu` (`MANGUYENLIEU`),
  ADD CONSTRAINT `FK_CTPN_PN` FOREIGN KEY (`MAPN`) REFERENCES `phieunhap` (`MAPN`);

--
-- Constraints for table `donhang`
--
ALTER TABLE `donhang`
  ADD CONSTRAINT `FK_DH_BAN` FOREIGN KEY (`MABAN`) REFERENCES `ban` (`MABAN`),
  ADD CONSTRAINT `FK_DH_ND` FOREIGN KEY (`TAOBOI`) REFERENCES `nguoidung` (`MANGUOIDUNG`);

--
-- Constraints for table `hoadon`
--
ALTER TABLE `hoadon`
  ADD CONSTRAINT `FK_HD_DH` FOREIGN KEY (`MADON`) REFERENCES `donhang` (`MADON`);

--
-- Constraints for table `nhatkykho`
--
ALTER TABLE `nhatkykho`
  ADD CONSTRAINT `FK_NK_NL` FOREIGN KEY (`MANGUYENLIEU`) REFERENCES `nguyenlieu` (`MANGUYENLIEU`);

--
-- Constraints for table `phieunhap`
--
ALTER TABLE `phieunhap`
  ADD CONSTRAINT `FK_PN_NCC` FOREIGN KEY (`MANCC`) REFERENCES `nhacungcap` (`MANCC`);

--
-- Constraints for table `sanpham`
--
ALTER TABLE `sanpham`
  ADD CONSTRAINT `FK_SP_DM` FOREIGN KEY (`MADANHMUC`) REFERENCES `danhmuc` (`MADANHMUC`);

--
-- Constraints for table `thanhtoan`
--
ALTER TABLE `thanhtoan`
  ADD CONSTRAINT `FK_TT_HD` FOREIGN KEY (`MAHOADON`) REFERENCES `hoadon` (`MAHOADON`);

--
-- Constraints for table `vaitronguoidung`
--
ALTER TABLE `vaitronguoidung`
  ADD CONSTRAINT `FK_NDVT_ND` FOREIGN KEY (`MANGUOIDUNG`) REFERENCES `nguoidung` (`MANGUOIDUNG`),
  ADD CONSTRAINT `FK_NDVT_VT` FOREIGN KEY (`MAVAITRO`) REFERENCES `vaitro` (`MAVAITRO`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
