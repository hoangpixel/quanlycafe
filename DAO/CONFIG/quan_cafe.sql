-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Oct 25, 2025 at 01:37 PM
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
  `MADONVICOSO` int(11) NOT NULL,
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `congthuc`
--

INSERT INTO `congthuc` (`MASANPHAM`, `MANGUYENLIEU`, `SOLUONGCOSO`, `MADONVICOSO`, `TRANGTHAI`) VALUES
(1, 1, 25.00, 1, 1),
(1, 2, 5.00, 1, 1),
(2, 1, 25.00, 1, 1),
(2, 3, 40.00, 1, 1),
(3, 1, 20.00, 1, 1),
(3, 2, 5.00, 1, 1),
(3, 3, 25.00, 1, 1),
(3, 4, 120.00, 3, 1),
(4, 5, 1.00, 5, 1);

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
  `MAPN` int(11) DEFAULT NULL,
  `MANGUYENLIEU` int(11) DEFAULT NULL,
  `MADONVI` int(11) DEFAULT NULL,
  `SOLUONG` decimal(10,2) DEFAULT NULL,
  `SOLUONGCOSO` decimal(10,2) DEFAULT NULL,
  `DONGIA` decimal(10,2) DEFAULT NULL,
  `THANHTIEN` decimal(10,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `donvi`
--

CREATE TABLE `donvi` (
  `MADONVI` int(11) NOT NULL,
  `TENDONVI` varchar(50) NOT NULL,
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `donvi`
--

INSERT INTO `donvi` (`MADONVI`, `TENDONVI`, `TRANGTHAI`) VALUES
(1, 'g', 1),
(2, 'kg', 1),
(3, 'ml', 1),
(4, 'lít', 1),
(5, 'lon', 1),
(6, 'chai', 1),
(7, 'lốc', 1),
(8, 'thùng', 1),
(9, 'hộp', 1);

-- --------------------------------------------------------

--
-- Table structure for table `hesodonvi`
--

CREATE TABLE `hesodonvi` (
  `MANGUYENLIEU` int(11) NOT NULL,
  `MADONVI` int(11) NOT NULL,
  `HESO` decimal(10,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `hesodonvi`
--

INSERT INTO `hesodonvi` (`MANGUYENLIEU`, `MADONVI`, `HESO`) VALUES
(1, 2, 1.00),
(2, 2, 1.00),
(3, 9, 1.00),
(4, 9, 1.00),
(5, 7, 6.00),
(5, 8, 24.00);

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

--
-- Dumping data for table `loai`
--

INSERT INTO `loai` (`MALOAI`, `TENLOAI`, `TRANGTHAI`) VALUES
(1, 'Cà phê', 1),
(2, 'Nước ngọt', 1);

-- --------------------------------------------------------

--
-- Table structure for table `nguyenlieu`
--

CREATE TABLE `nguyenlieu` (
  `MANGUYENLIEU` int(11) NOT NULL,
  `TENNGUYENLIEU` varchar(120) NOT NULL,
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 1,
  `TONKHO` decimal(14,3) NOT NULL DEFAULT 0.000,
  `TRANGTHAIDV` tinyint(1) NOT NULL DEFAULT 0,
  `MADONVICOSO` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `nguyenlieu`
--

INSERT INTO `nguyenlieu` (`MANGUYENLIEU`, `TENNGUYENLIEU`, `TRANGTHAI`, `TONKHO`, `TRANGTHAIDV`, `MADONVICOSO`) VALUES
(1, 'Bột cà phê', 1, 0.000, 1, 2),
(2, 'Đường trắng', 1, 0.000, 1, 2),
(3, 'Sữa đặc', 1, 0.000, 1, 2),
(4, 'Sữa tươi không đường', 1, 0.000, 1, 4),
(5, 'Coca cola', 1, 0.000, 1, 5);

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

--
-- Dumping data for table `sanpham`
--

INSERT INTO `sanpham` (`MASANPHAM`, `MALOAI`, `HINH`, `TENSANPHAM`, `TRANGTHAI`, `TRANGTHAICT`, `GIA`) VALUES
(1, 1, 'cafe-den-da-8801.png', 'Cà phê đen', 1, 1, 17000.00),
(2, 1, 'pngtree-ice-milk-coffee-png-image_9162395.png', 'Cà phê sữa', 1, 1, 25000.00),
(3, 1, 'pngtree-cute-iced-coffee-takeaway-png-image_11477425.png', 'Cà phê sữa tươi', 1, 1, 25000.00),
(4, 2, 's-1-lon-cocacola.png', 'Coca cola lon', 1, 1, 15000.00);

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
  ADD PRIMARY KEY (`MASANPHAM`,`MANGUYENLIEU`,`MADONVICOSO`),
  ADD KEY `FK_CT_NL` (`MANGUYENLIEU`),
  ADD KEY `fk_congthuc_donvi` (`MADONVICOSO`);

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
  ADD KEY `MAPN` (`MAPN`),
  ADD KEY `MANGUYENLIEU` (`MANGUYENLIEU`),
  ADD KEY `MADONVI` (`MADONVI`);

--
-- Indexes for table `donvi`
--
ALTER TABLE `donvi`
  ADD PRIMARY KEY (`MADONVI`);

--
-- Indexes for table `hesodonvi`
--
ALTER TABLE `hesodonvi`
  ADD PRIMARY KEY (`MANGUYENLIEU`,`MADONVI`),
  ADD KEY `MADONVI` (`MADONVI`);

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
  ADD UNIQUE KEY `uq_nl_ten` (`TENNGUYENLIEU`),
  ADD KEY `MADONVICOSO` (`MADONVICOSO`);

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
-- AUTO_INCREMENT for table `donvi`
--
ALTER TABLE `donvi`
  MODIFY `MADONVI` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

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
  MODIFY `MALOAI` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `nguyenlieu`
--
ALTER TABLE `nguyenlieu`
  MODIFY `MANGUYENLIEU` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

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
  MODIFY `MASANPHAM` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

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
  ADD CONSTRAINT `FK_CT_SP` FOREIGN KEY (`MASANPHAM`) REFERENCES `sanpham` (`MASANPHAM`),
  ADD CONSTRAINT `fk_congthuc_donvi` FOREIGN KEY (`MADONVICOSO`) REFERENCES `donvi` (`MADONVI`);

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
  ADD CONSTRAINT `ctphieunhap_ibfk_1` FOREIGN KEY (`MAPN`) REFERENCES `phieunhap` (`MAPN`),
  ADD CONSTRAINT `ctphieunhap_ibfk_2` FOREIGN KEY (`MANGUYENLIEU`) REFERENCES `nguyenlieu` (`MANGUYENLIEU`),
  ADD CONSTRAINT `ctphieunhap_ibfk_3` FOREIGN KEY (`MADONVI`) REFERENCES `donvi` (`MADONVI`);

--
-- Constraints for table `hesodonvi`
--
ALTER TABLE `hesodonvi`
  ADD CONSTRAINT `hesodonvi_ibfk_1` FOREIGN KEY (`MANGUYENLIEU`) REFERENCES `nguyenlieu` (`MANGUYENLIEU`),
  ADD CONSTRAINT `hesodonvi_ibfk_2` FOREIGN KEY (`MADONVI`) REFERENCES `donvi` (`MADONVI`);

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
-- Constraints for table `nguyenlieu`
--
ALTER TABLE `nguyenlieu`
  ADD CONSTRAINT `nguyenlieu_ibfk_1` FOREIGN KEY (`MADONVICOSO`) REFERENCES `donvi` (`MADONVI`);

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
