-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Oct 14, 2025 at 04:16 AM
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
-- Database: `quan_cafe`
--

-- --------------------------------------------------------

--
-- Table structure for table `ban`
--

CREATE TABLE `ban` (
  `MABAN` int(11) NOT NULL,
  `TENBAN` varchar(100) NOT NULL,
  `DANGSUDUNG` tinyint(1) NOT NULL DEFAULT 1,
  `MADONHIENTAI` int(11) DEFAULT NULL,
  `MAKHUVUC` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `calam`
--

CREATE TABLE `calam` (
  `MACA` int(11) NOT NULL,
  `TENCA` varchar(60) NOT NULL,
  `THOIGIANBD` time NOT NULL,
  `THOIGIANKT` time NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `congthuc`
--

CREATE TABLE `congthuc` (
  `MASANPHAM` int(11) NOT NULL,
  `MANGUYENLIEU` int(11) NOT NULL,
  `SOLUONGCOSO` decimal(12,2) NOT NULL,
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `cthd`
--

CREATE TABLE `cthd` (
  `MACTHD` int(11) NOT NULL,
  `MAHOADON` int(11) NOT NULL,
  `MASANPHAM` int(11) NOT NULL,
  `SOLUONG` int(11) NOT NULL CHECK (`SOLUONG` > 0),
  `DONGIA` decimal(12,2) NOT NULL,
  `THANHTIEN` decimal(12,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Triggers `cthd`
--
DELIMITER $$
CREATE TRIGGER `trg_cthd_ad` AFTER DELETE ON `cthd` FOR EACH ROW BEGIN
  UPDATE HOADON SET TONGTIEN = TONGTIEN - OLD.THANHTIEN WHERE MAHOADON = OLD.MAHOADON;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trg_cthd_ai` AFTER INSERT ON `cthd` FOR EACH ROW BEGIN
  UPDATE HOADON SET TONGTIEN = TONGTIEN + NEW.THANHTIEN WHERE MAHOADON = NEW.MAHOADON;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `ctphieunhap`
--

CREATE TABLE `ctphieunhap` (
  `MACTPN` int(11) NOT NULL,
  `MAPN` int(11) NOT NULL,
  `MANGUYENLIEU` int(11) NOT NULL,
  `SOLUONGCOSO` decimal(12,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `hoadon`
--

CREATE TABLE `hoadon` (
  `MAHOADON` int(11) NOT NULL,
  `MABAN` int(11) DEFAULT NULL,
  `MATT` int(11) DEFAULT NULL,
  `THOIGIANTAO` datetime NOT NULL DEFAULT current_timestamp(),
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 0,
  `TONGTIEN` decimal(12,2) NOT NULL DEFAULT 0.00,
  `MAKHACHHANG` int(11) DEFAULT NULL,
  `MANHANVIEN` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `khachhang`
--

CREATE TABLE `khachhang` (
  `MAKHACHHANG` int(11) NOT NULL,
  `TENKHACHHANG` varchar(120) NOT NULL,
  `SODIENTHOAI` varchar(20) DEFAULT NULL,
  `EMAIL` varchar(120) DEFAULT NULL,
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 1,
  `NGAYTAO` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `khuvuc`
--

CREATE TABLE `khuvuc` (
  `MAKHUVUC` int(11) NOT NULL,
  `TENKHUVUC` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `lichlamviec`
--

CREATE TABLE `lichlamviec` (
  `MANHANVIEN` int(11) NOT NULL,
  `NGAY` date NOT NULL,
  `MACA` int(11) NOT NULL,
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `loai`
--

CREATE TABLE `loai` (
  `MALOAI` int(11) NOT NULL,
  `TENLOAI` varchar(100) NOT NULL,
  `TRANGTHAI` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `nguyenlieu`
--

CREATE TABLE `nguyenlieu` (
  `MANGUYENLIEU` int(11) NOT NULL,
  `TENNGUYENLIEU` varchar(120) NOT NULL,
  `DONVICOSO` varchar(30) NOT NULL,
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 1,
  `TONKHO` decimal(14,3) NOT NULL DEFAULT 0.000
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `nhacungcap`
--

CREATE TABLE `nhacungcap` (
  `MANCC` int(11) NOT NULL,
  `TENNCC` varchar(150) NOT NULL,
  `SODIENTHOAI` varchar(20) DEFAULT NULL,
  `EMAIL` varchar(120) DEFAULT NULL,
  `DIACHI` varchar(255) DEFAULT NULL,
  `CONHOATDONG` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `nhanvien`
--

CREATE TABLE `nhanvien` (
  `MANHANVIEN` int(11) NOT NULL,
  `HOTEN` varchar(120) NOT NULL,
  `SODIENTHOAI` varchar(20) DEFAULT NULL,
  `EMAIL` varchar(120) DEFAULT NULL,
  `LUONG` decimal(12,2) DEFAULT 0.00,
  `NGAYTAO` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `phieunhap`
--

CREATE TABLE `phieunhap` (
  `MAPN` int(11) NOT NULL,
  `MANCC` int(11) NOT NULL,
  `THOIGIAN` datetime NOT NULL DEFAULT current_timestamp(),
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `sanpham`
--

CREATE TABLE `sanpham` (
  `MASANPHAM` int(11) NOT NULL,
  `MALOAI` int(11) NOT NULL,
  `HINH` varchar(255) DEFAULT NULL,
  `TENSANPHAM` varchar(120) NOT NULL,
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 1,
  `TRANGTHAICT` tinyint(1) NOT NULL DEFAULT 0,
  `GIA` decimal(12,2) NOT NULL DEFAULT 0.00
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `taikhoan`
--

CREATE TABLE `taikhoan` (
  `MATAIKHOAN` int(11) NOT NULL,
  `MANHANVIEN` int(11) NOT NULL,
  `TENDANGNHAP` varchar(60) NOT NULL,
  `MATKHAU` varchar(255) NOT NULL,
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 1,
  `NGAYTAO` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `thanhtoan`
--

CREATE TABLE `thanhtoan` (
  `MATT` int(11) NOT NULL,
  `HINHTHUC` varchar(60) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `vaitro`
--

CREATE TABLE `vaitro` (
  `MAVAITRO` int(11) NOT NULL,
  `TENVAITRO` varchar(60) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `vaitronguoidung`
--

CREATE TABLE `vaitronguoidung` (
  `MANHANVIEN` int(11) NOT NULL,
  `MAVAITRO` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `ban`
--
ALTER TABLE `ban`
  ADD PRIMARY KEY (`MABAN`),
  ADD KEY `FK_BAN_KHUVUC` (`MAKHUVUC`);

--
-- Indexes for table `calam`
--
ALTER TABLE `calam`
  ADD PRIMARY KEY (`MACA`);

--
-- Indexes for table `congthuc`
--
ALTER TABLE `congthuc`
  ADD PRIMARY KEY (`MASANPHAM`,`MANGUYENLIEU`),
  ADD KEY `FK_CT_NL` (`MANGUYENLIEU`);

--
-- Indexes for table `cthd`
--
ALTER TABLE `cthd`
  ADD PRIMARY KEY (`MACTHD`),
  ADD UNIQUE KEY `uq_cthd` (`MAHOADON`,`MASANPHAM`),
  ADD KEY `FK_CTHD_SP` (`MASANPHAM`);

--
-- Indexes for table `ctphieunhap`
--
ALTER TABLE `ctphieunhap`
  ADD PRIMARY KEY (`MACTPN`),
  ADD UNIQUE KEY `uq_ctpn` (`MAPN`,`MANGUYENLIEU`),
  ADD KEY `FK_CTPN_NL` (`MANGUYENLIEU`);

--
-- Indexes for table `hoadon`
--
ALTER TABLE `hoadon`
  ADD PRIMARY KEY (`MAHOADON`),
  ADD KEY `FK_HD_BAN` (`MABAN`),
  ADD KEY `FK_HD_TT` (`MATT`),
  ADD KEY `FK_HD_KH` (`MAKHACHHANG`),
  ADD KEY `FK_HD_NV` (`MANHANVIEN`),
  ADD KEY `idx_hd_time` (`THOIGIANTAO`);

--
-- Indexes for table `khachhang`
--
ALTER TABLE `khachhang`
  ADD PRIMARY KEY (`MAKHACHHANG`),
  ADD UNIQUE KEY `uq_kh_sdt` (`SODIENTHOAI`),
  ADD UNIQUE KEY `uq_kh_email` (`EMAIL`);

--
-- Indexes for table `khuvuc`
--
ALTER TABLE `khuvuc`
  ADD PRIMARY KEY (`MAKHUVUC`),
  ADD UNIQUE KEY `uq_khuvuc_ten` (`TENKHUVUC`);

--
-- Indexes for table `lichlamviec`
--
ALTER TABLE `lichlamviec`
  ADD PRIMARY KEY (`MANHANVIEN`,`NGAY`,`MACA`),
  ADD KEY `FK_LLV_CA` (`MACA`);

--
-- Indexes for table `loai`
--
ALTER TABLE `loai`
  ADD PRIMARY KEY (`MALOAI`),
  ADD UNIQUE KEY `uq_loai_ten` (`TENLOAI`);

--
-- Indexes for table `nguyenlieu`
--
ALTER TABLE `nguyenlieu`
  ADD PRIMARY KEY (`MANGUYENLIEU`),
  ADD UNIQUE KEY `uq_nl_ten` (`TENNGUYENLIEU`);

--
-- Indexes for table `nhacungcap`
--
ALTER TABLE `nhacungcap`
  ADD PRIMARY KEY (`MANCC`);

--
-- Indexes for table `nhanvien`
--
ALTER TABLE `nhanvien`
  ADD PRIMARY KEY (`MANHANVIEN`),
  ADD UNIQUE KEY `uq_nhanvien_email` (`EMAIL`);

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
  ADD KEY `FK_SANPHAM_LOAI` (`MALOAI`),
  ADD KEY `idx_sp_ten` (`TENSANPHAM`);

--
-- Indexes for table `taikhoan`
--
ALTER TABLE `taikhoan`
  ADD PRIMARY KEY (`MATAIKHOAN`),
  ADD UNIQUE KEY `MANHANVIEN` (`MANHANVIEN`),
  ADD UNIQUE KEY `TENDANGNHAP` (`TENDANGNHAP`);

--
-- Indexes for table `thanhtoan`
--
ALTER TABLE `thanhtoan`
  ADD PRIMARY KEY (`MATT`);

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
  ADD PRIMARY KEY (`MANHANVIEN`,`MAVAITRO`),
  ADD KEY `FK_VTND_VT` (`MAVAITRO`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `ban`
--
ALTER TABLE `ban`
  MODIFY `MABAN` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `calam`
--
ALTER TABLE `calam`
  MODIFY `MACA` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `cthd`
--
ALTER TABLE `cthd`
  MODIFY `MACTHD` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `ctphieunhap`
--
ALTER TABLE `ctphieunhap`
  MODIFY `MACTPN` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `hoadon`
--
ALTER TABLE `hoadon`
  MODIFY `MAHOADON` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `khachhang`
--
ALTER TABLE `khachhang`
  MODIFY `MAKHACHHANG` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `khuvuc`
--
ALTER TABLE `khuvuc`
  MODIFY `MAKHUVUC` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `loai`
--
ALTER TABLE `loai`
  MODIFY `MALOAI` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `nguyenlieu`
--
ALTER TABLE `nguyenlieu`
  MODIFY `MANGUYENLIEU` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `nhacungcap`
--
ALTER TABLE `nhacungcap`
  MODIFY `MANCC` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `nhanvien`
--
ALTER TABLE `nhanvien`
  MODIFY `MANHANVIEN` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `phieunhap`
--
ALTER TABLE `phieunhap`
  MODIFY `MAPN` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `sanpham`
--
ALTER TABLE `sanpham`
  MODIFY `MASANPHAM` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `taikhoan`
--
ALTER TABLE `taikhoan`
  MODIFY `MATAIKHOAN` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `thanhtoan`
--
ALTER TABLE `thanhtoan`
  MODIFY `MATT` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `vaitro`
--
ALTER TABLE `vaitro`
  MODIFY `MAVAITRO` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `ban`
--
ALTER TABLE `ban`
  ADD CONSTRAINT `FK_BAN_KHUVUC` FOREIGN KEY (`MAKHUVUC`) REFERENCES `khuvuc` (`MAKHUVUC`);

--
-- Constraints for table `congthuc`
--
ALTER TABLE `congthuc`
  ADD CONSTRAINT `FK_CT_NL` FOREIGN KEY (`MANGUYENLIEU`) REFERENCES `nguyenlieu` (`MANGUYENLIEU`),
  ADD CONSTRAINT `FK_CT_SP` FOREIGN KEY (`MASANPHAM`) REFERENCES `sanpham` (`MASANPHAM`);

--
-- Constraints for table `cthd`
--
ALTER TABLE `cthd`
  ADD CONSTRAINT `FK_CTHD_HD` FOREIGN KEY (`MAHOADON`) REFERENCES `hoadon` (`MAHOADON`),
  ADD CONSTRAINT `FK_CTHD_SP` FOREIGN KEY (`MASANPHAM`) REFERENCES `sanpham` (`MASANPHAM`);

--
-- Constraints for table `ctphieunhap`
--
ALTER TABLE `ctphieunhap`
  ADD CONSTRAINT `FK_CTPN_NL` FOREIGN KEY (`MANGUYENLIEU`) REFERENCES `nguyenlieu` (`MANGUYENLIEU`),
  ADD CONSTRAINT `FK_CTPN_PN` FOREIGN KEY (`MAPN`) REFERENCES `phieunhap` (`MAPN`);

--
-- Constraints for table `hoadon`
--
ALTER TABLE `hoadon`
  ADD CONSTRAINT `FK_HD_BAN` FOREIGN KEY (`MABAN`) REFERENCES `ban` (`MABAN`),
  ADD CONSTRAINT `FK_HD_KH` FOREIGN KEY (`MAKHACHHANG`) REFERENCES `khachhang` (`MAKHACHHANG`),
  ADD CONSTRAINT `FK_HD_NV` FOREIGN KEY (`MANHANVIEN`) REFERENCES `nhanvien` (`MANHANVIEN`),
  ADD CONSTRAINT `FK_HD_TT` FOREIGN KEY (`MATT`) REFERENCES `thanhtoan` (`MATT`);

--
-- Constraints for table `lichlamviec`
--
ALTER TABLE `lichlamviec`
  ADD CONSTRAINT `FK_LLV_CA` FOREIGN KEY (`MACA`) REFERENCES `calam` (`MACA`),
  ADD CONSTRAINT `FK_LLV_NV` FOREIGN KEY (`MANHANVIEN`) REFERENCES `nhanvien` (`MANHANVIEN`);

--
-- Constraints for table `phieunhap`
--
ALTER TABLE `phieunhap`
  ADD CONSTRAINT `FK_PN_NCC` FOREIGN KEY (`MANCC`) REFERENCES `nhacungcap` (`MANCC`);

--
-- Constraints for table `sanpham`
--
ALTER TABLE `sanpham`
  ADD CONSTRAINT `FK_SANPHAM_LOAI` FOREIGN KEY (`MALOAI`) REFERENCES `loai` (`MALOAI`);

--
-- Constraints for table `taikhoan`
--
ALTER TABLE `taikhoan`
  ADD CONSTRAINT `FK_TAIKHOAN_NHANVIEN` FOREIGN KEY (`MANHANVIEN`) REFERENCES `nhanvien` (`MANHANVIEN`);

--
-- Constraints for table `vaitronguoidung`
--
ALTER TABLE `vaitronguoidung`
  ADD CONSTRAINT `FK_VTND_NV` FOREIGN KEY (`MANHANVIEN`) REFERENCES `nhanvien` (`MANHANVIEN`),
  ADD CONSTRAINT `FK_VTND_VT` FOREIGN KEY (`MAVAITRO`) REFERENCES `vaitro` (`MAVAITRO`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
