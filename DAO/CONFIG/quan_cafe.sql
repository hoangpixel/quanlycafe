-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 12, 2025 at 07:49 AM
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
  `MAKHUVUC` int(11) NOT NULL,
  `TRANGTHAIXOA` tinyint(1) NOT NULL DEFAULT 1,
  `MADONHIENTAI` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `ban`
--

INSERT INTO `ban` (`MABAN`, `TENBAN`, `DANGSUDUNG`, `MAKHUVUC`, `TRANGTHAIXOA`, `MADONHIENTAI`) VALUES
(1, 'Bàn 1', 0, 1, 1, NULL),
(2, 'Bàn 2', 1, 1, 1, NULL),
(3, 'Bàn 3', 1, 1, 1, NULL),
(4, 'Bàn 4', 1, 1, 1, NULL),
(5, 'Bàn 5', 1, 1, 1, NULL),
(6, 'Bàn 6', 1, 1, 1, NULL),
(7, 'Bàn 7', 1, 1, 1, NULL),
(8, 'Bàn 8', 1, 1, 1, NULL),
(9, 'Bàn 9', 1, 1, 1, NULL),
(10, 'Bàn 10', 1, 1, 1, NULL),
(11, 'Bàn 11', 1, 1, 1, NULL),
(12, 'Bàn 12', 1, 1, 1, NULL),
(13, 'Bàn 13', 1, 1, 1, NULL),
(14, 'Bàn 14', 1, 1, 1, NULL),
(15, 'Bàn 15', 1, 1, 1, NULL),
(16, 'Bàn 16', 1, 1, 1, NULL),
(17, 'Bàn 17', 1, 1, 1, NULL),
(18, 'Bàn 18', 1, 1, 1, NULL),
(19, 'Bàn 19', 1, 1, 1, NULL),
(20, 'Bàn 20', 1, 1, 1, NULL),
(21, 'Bàn 21', 0, 2, 1, NULL),
(22, 'Bàn 22', 1, 2, 1, NULL),
(23, 'Bàn 23', 1, 2, 1, NULL),
(24, 'Bàn 24', 1, 2, 1, NULL),
(25, 'Bàn 25', 1, 2, 1, NULL),
(26, 'Bàn 26', 1, 2, 1, NULL),
(27, 'Bàn 27', 1, 2, 1, NULL),
(28, 'Bàn 28', 1, 2, 1, NULL),
(29, 'Bàn 29', 1, 2, 1, NULL),
(30, 'Bàn 30', 1, 2, 1, NULL),
(31, 'Bàn 31', 1, 2, 1, NULL),
(32, 'Bàn 32', 1, 2, 1, NULL),
(33, 'Bàn 33', 1, 2, 1, NULL),
(34, 'Bàn 34', 1, 2, 1, NULL),
(35, 'Bàn 35', 1, 2, 1, NULL),
(36, 'Bàn 36', 1, 2, 1, NULL),
(37, 'Bàn 37', 1, 2, 1, NULL),
(38, 'Bàn 38', 1, 2, 1, NULL),
(39, 'Bàn 39', 1, 2, 1, NULL),
(40, 'Bàn 40', 1, 2, 1, NULL);

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
(4, 5, 1.00, 5, 1),
(6, 6, 500.00, 1, 1),
(6, 7, 300.00, 1, 1),
(7, 7, 300.00, 1, 1),
(7, 8, 400.00, 1, 1);

-- --------------------------------------------------------

--
-- Table structure for table `cthd`
--

CREATE TABLE `cthd` (
  `MAHOADON` int(11) NOT NULL,
  `MASANPHAM` int(11) NOT NULL,
  `SOLUONG` int(11) NOT NULL CHECK (`SOLUONG` > 0),
  `DONGIA` decimal(12,2) NOT NULL,
  `THANHTIEN` decimal(12,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `cthd`
--

INSERT INTO `cthd` (`MAHOADON`, `MASANPHAM`, `SOLUONG`, `DONGIA`, `THANHTIEN`) VALUES
(1, 1, 1, 17000.00, 17000.00),
(2, 3, 4, 25000.00, 100000.00),
(3, 3, 2, 25000.00, 50000.00),
(4, 1, 1, 17000.00, 17000.00),
(4, 4, 4, 15000.00, 60000.00),
(5, 1, 1, 17000.00, 17000.00),
(5, 2, 1, 25000.00, 25000.00),
(5, 3, 1, 25000.00, 25000.00),
(5, 4, 1, 15000.00, 15000.00),
(6, 1, 1, 17000.00, 17000.00),
(7, 1, 1, 17000.00, 17000.00),
(7, 3, 4, 25000.00, 100000.00),
(8, 4, 5, 15000.00, 75000.00),
(9, 1, 1, 17000.00, 17000.00),
(9, 2, 1, 25000.00, 25000.00),
(9, 3, 1, 25000.00, 25000.00),
(9, 4, 1, 15000.00, 15000.00),
(10, 1, 1, 17000.00, 17000.00),
(10, 2, 1, 25000.00, 25000.00),
(11, 1, 1, 17000.00, 17000.00),
(12, 4, 1, 15000.00, 15000.00),
(13, 4, 4, 15000.00, 60000.00),
(13, 7, 1, 50000.00, 50000.00);

-- --------------------------------------------------------

--
-- Table structure for table `ctphieunhap`
--

CREATE TABLE `ctphieunhap` (
  `MAPN` int(11) NOT NULL,
  `MANGUYENLIEU` int(11) NOT NULL,
  `MADONVI` int(11) NOT NULL,
  `SOLUONG` decimal(10,2) NOT NULL DEFAULT 0.00,
  `SOLUONGCOSO` decimal(10,2) NOT NULL DEFAULT 0.00,
  `DONGIA` decimal(12,2) NOT NULL DEFAULT 0.00,
  `THANHTIEN` decimal(12,2) NOT NULL DEFAULT 0.00
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `ctphieunhap`
--

INSERT INTO `ctphieunhap` (`MAPN`, `MANGUYENLIEU`, `MADONVI`, `SOLUONG`, `SOLUONGCOSO`, `DONGIA`, `THANHTIEN`) VALUES
(1, 5, 7, 10.00, 60.00, 60000.00, 600000.00),
(2, 5, 5, 2.00, 2.00, 10000.00, 20000.00),
(3, 1, 2, 50.00, 50.00, 150000.00, 7500000.00),
(4, 3, 2, 50.00, 50.00, 70000.00, 3500000.00),
(5, 3, 2, 55.00, 55.00, 78000.00, 4290000.00),
(5, 5, 8, 5.00, 120.00, 150000.00, 750000.00),
(6, 4, 4, 50.00, 50.00, 85000.00, 4250000.00),
(7, 2, 2, 50.00, 50.00, 30000.00, 1500000.00),
(8, 6, 2, 50.00, 50.00, 800000.00, 40000000.00),
(8, 7, 2, 50.00, 50.00, 70000.00, 3500000.00),
(8, 8, 2, 50.00, 50.00, 600000.00, 30000000.00);

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
  `HESO` decimal(10,3) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `hesodonvi`
--

INSERT INTO `hesodonvi` (`MANGUYENLIEU`, `MADONVI`, `HESO`) VALUES
(1, 1, 0.001),
(1, 2, 1.000),
(2, 1, 0.001),
(2, 2, 1.000),
(3, 1, 0.001),
(3, 2, 1.000),
(3, 9, 0.600),
(4, 3, 0.001),
(4, 4, 1.000),
(4, 9, 0.800),
(5, 5, 1.000),
(5, 7, 6.000),
(5, 8, 24.000),
(6, 1, 0.001),
(6, 2, 1.000),
(7, 1, 0.001),
(7, 2, 1.000),
(8, 1, 0.001),
(8, 2, 1.000);

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
  `MANHANVIEN` int(11) NOT NULL,
  `TRANGTHAIXOA` tinyint(1) NOT NULL DEFAULT 1,
  `KHOASO` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `hoadon`
--

INSERT INTO `hoadon` (`MAHOADON`, `MABAN`, `MATT`, `THOIGIANTAO`, `TRANGTHAI`, `TONGTIEN`, `MAKHACHHANG`, `MANHANVIEN`, `TRANGTHAIXOA`, `KHOASO`) VALUES
(1, 1, 1, '2025-09-12 13:13:42', 1, 17000.00, 0, 1, 1, 1),
(2, 2, 1, '2025-10-10 13:15:09', 1, 100000.00, 0, 1, 1, 1),
(3, 3, 1, '2025-10-14 13:15:20', 1, 50000.00, 0, 1, 1, 1),
(4, 4, 1, '2025-11-01 13:15:31', 1, 77000.00, 1, 1, 1, 1),
(5, 5, 1, '2025-11-02 13:16:11', 1, 82000.00, 0, 1, 1, 1),
(6, 6, 1, '2025-11-04 13:17:27', 1, 17000.00, 0, 1, 1, 1),
(7, 7, 3, '2025-11-05 13:18:46', 1, 117000.00, 2, 1, 1, 1),
(8, 8, 1, '2025-11-11 13:22:49', 1, 75000.00, 6, 1, 1, 1),
(9, 9, 1, '2025-12-01 13:26:24', 1, 82000.00, 0, 1, 1, 1),
(10, 10, 1, '2025-12-05 13:26:38', 1, 42000.00, 10, 1, 1, 1),
(11, 21, 1, CONCAT(CURDATE(), ' 05:00:00'), 1, 17000.00, 6, 1, 1, 0),
(12, 21, 1, CONCAT(CURDATE(), ' 05:30:00'), 1, 15000.00, 6, 1, 1, 0),
(13, 1, 3, CONCAT(CURDATE(), ' 06:00:00'), 1, 110000.00, 6, 1, 1, 0);

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

--
-- Dumping data for table `khachhang`
--

INSERT INTO `khachhang` (`MAKHACHHANG`, `TENKHACHHANG`, `SODIENTHOAI`, `EMAIL`, `TRANGTHAI`, `NGAYTAO`) VALUES
(0, 'Khách lẻ', NULL, NULL, 0, '2025-12-08 19:13:16'),
(1, 'Nguyễn Văn A', '0333333333', 'khachhang1@gmail.com', 1, '2025-11-14 13:17:26'),
(2, 'Xuân Trường Bắc Giang', '0333333334', 'khachhang2@gmail.com', 1, '2025-12-11 13:34:04'),
(3, 'Lê Xuân Trường', '0222222222', 'khachhang3@gmail.com', 1, '2025-12-11 13:38:31'),
(4, 'Phạm Thị Hương', '0911223344', 'khachhang4@gmail.com', 1, '2025-12-11 13:44:02'),
(5, 'Trần Văn Nam', '0922334455', 'khachhang5@gmail.com', 1, '2025-12-11 13:44:02'),
(6, 'Lê Thị Lan', '0933445566', 'khachhang6@gmail.com', 1, '2025-12-11 13:44:02'),
(7, 'Hoàng Văn Tuấn', '0944556677', 'khachhang7@gmail.com', 1, '2025-12-11 13:44:02'),
(8, 'Đỗ Thị Minh', '0955667788', 'khachhang8@gmail.com', 1, '2025-12-11 13:44:02'),
(9, 'Ngô Văn Hùng', '0966778899', 'khachhang9@gmail.com', 1, '2025-12-11 13:44:02'),
(10, 'Bùi Thị Mai', '0977889900', 'khachhang10@gmail.com', 1, '2025-12-11 13:44:02');

-- --------------------------------------------------------

--
-- Table structure for table `khuvuc`
--

CREATE TABLE `khuvuc` (
  `MAKHUVUC` int(11) NOT NULL,
  `TENKHUVUC` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `khuvuc`
--

INSERT INTO `khuvuc` (`MAKHUVUC`, `TENKHUVUC`) VALUES
(1, 'A'),
(2, 'B');

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
  `MANHOM` int(11) DEFAULT NULL,
  `TENLOAI` varchar(100) NOT NULL,
  `TRANGTHAI` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `loai`
--

INSERT INTO `loai` (`MALOAI`, `MANHOM`, `TENLOAI`, `TRANGTHAI`) VALUES
(1, 1, 'Cà phê', 1),
(2, 1, 'Nước ngọt', 1),
(3, 2, 'Cơm chiên', 1),
(4, 1, 'Cơm', 1);

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
(1, 'Bột cà phê', 1, 49.485, 1, 2),
(2, 'Đường trắng', 1, 49.900, 1, 2),
(3, 'Sữa đặc', 1, 104.580, 1, 2),
(4, 'Sữa tươi không đường', 1, 48.560, 1, 4),
(5, 'Coca cola', 1, 166.000, 1, 5),
(6, 'Sườn', 1, 50.000, 1, 2),
(7, 'Cơm', 1, 49.700, 1, 2),
(8, 'Gà', 1, 49.600, 1, 2);

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

--
-- Dumping data for table `nhacungcap`
--

INSERT INTO `nhacungcap` (`MANCC`, `TENNCC`, `SODIENTHOAI`, `EMAIL`, `DIACHI`, `CONHOATDONG`) VALUES
(1, 'Lê Thị Cường', '0123123123', 'ncc1@gmail.com', '123 Đường Láng, Đống Đa, Hà Nội', 1),
(2, 'Nguyễn Xuân Mai', '0123123124', 'ncc2@gmail.com', '456 Nguyễn Trãi, Quận 5, TP.HCM', 1),
(3, 'Công ty Thực Phẩm Sạch', '0911223344', 'ncc3@gmail.com', '789 Trần Hưng Đạo, Đà Nẵng', 1),
(4, 'Đại Lý Nước Ngọt Hùng Dũng', '0922334455', 'ncc4@gmail.com', '12 Lê Lợi, TP Huế', 1),
(5, 'Nông Sản Việt Green', '0933445566', 'ncc5@gmail.com', '34 Pasteur, Quận 1, TP.HCM', 1),
(6, 'Công Ty TNHH MTV An Khang', '0944556677', 'ncc6@gmail.com', '56 Cầu Giấy, Hà Nội', 1),
(7, 'Nhà Phân Phối Bánh Kẹo Á Châu', '0955667788', 'ncc7@gmail.com', '88 Lý Thường Kiệt, Cần Thơ', 1),
(8, 'Công Ty Cà Phê Cao Nguyên', '0966778899', 'ncc8@gmail.com', 'Buôn Ma Thuột, Đắk Lắk', 1),
(9, 'Hải Sản Biển Đông', '0977889900', 'ncc9@gmail.com', '102 Bạch Đằng, Nha Trang', 1),
(10, 'Công Ty Sữa Và Chế Phẩm', '0988990011', 'ncc10@gmail.com', 'KCN Sóng Thần, Bình Dương', 1);

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
  `NGAYTAO` datetime NOT NULL DEFAULT current_timestamp(),
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `nhanvien`
--

INSERT INTO `nhanvien` (`MANHANVIEN`, `HOTEN`, `SODIENTHOAI`, `EMAIL`, `LUONG`, `NGAYTAO`, `TRANGTHAI`) VALUES
(1, 'Admin', '0333333333', 'admin1@gmail.com', 30000000.00, '2025-11-14 13:17:41', 1),
(2, 'Nguyễn Văn A', '0123456789', 'nhanvien1@gmail.com', 7000000.00, '2025-12-05 14:49:45', 1),
(3, 'Trần Văn B', '0222222222', 'nhanvien2@gmail.com', 6500000.00, '2025-12-05 16:18:25', 1),
(4, 'Phạm Minh Hoàng', '0333333334', 'nhanvien3@gmail.com', 5500000.00, '2025-12-11 13:16:27', 1),
(5, 'Nguyễn Tuấn Trường', '0111111111', 'nhanvien4@gmail.com', 5500000.00, '2025-12-11 13:18:08', 1),
(6, 'Phạm Lê Gia Lai', '0999999999', 'nhanvien5@gmail.com', 6000000.00, '2025-12-11 13:22:16', 1),
(7, 'Trần Thị Lan', '0987654321', 'nhanvien6@gmail.com', 7000000.00, '2025-12-11 13:31:21', 1),
(8, 'Lê Thị Hường', '0898978783', 'nhanvien7@gmail.com', 3500000.00, '2025-12-11 13:31:57', 1),
(9, 'Trần Ri Cha', '05675433123', 'nhanvien8@gmail.com', 4000000.00, '2025-12-11 13:32:35', 1),
(10, 'Kim Trần Nang', '0678232123', 'nhanvien9@gmail.com', 3400000.00, '2025-12-11 13:33:21', 1);

-- --------------------------------------------------------

--
-- Table structure for table `nhom`
--

CREATE TABLE `nhom` (
  `MANHOM` int(11) NOT NULL,
  `TENNHOM` varchar(100) NOT NULL,
  `TRANGTHAI` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `nhom`
--

INSERT INTO `nhom` (`MANHOM`, `TENNHOM`, `TRANGTHAI`) VALUES
(1, 'Đồ uống', 1),
(2, 'Đồ ăn', 1),
(3, 'Khác', 1);

-- --------------------------------------------------------

--
-- Table structure for table `phanquyen`
--

CREATE TABLE `phanquyen` (
  `MAVAITRO` int(11) NOT NULL,
  `MAQUYEN` int(11) NOT NULL,
  `CAN_READ` tinyint(1) DEFAULT 0,
  `CAN_CREATE` tinyint(1) DEFAULT 0,
  `CAN_UPDATE` tinyint(1) DEFAULT 0,
  `CAN_DELETE` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `phanquyen`
--

INSERT INTO `phanquyen` (`MAVAITRO`, `MAQUYEN`, `CAN_READ`, `CAN_CREATE`, `CAN_UPDATE`, `CAN_DELETE`) VALUES
(1, 1, 1, 1, 1, 1),
(1, 2, 1, 1, 1, 1),
(1, 3, 1, 1, 1, 1),
(1, 4, 1, 1, 1, 1),
(1, 5, 1, 1, 1, 1),
(1, 6, 1, 1, 1, 1),
(1, 7, 1, 1, 1, 1),
(1, 8, 1, 1, 1, 1),
(2, 1, 1, 1, 1, 1),
(2, 2, 1, 1, 1, 1),
(2, 3, 1, 1, 1, 1),
(2, 4, 1, 1, 1, 1),
(2, 5, 0, 0, 0, 0),
(2, 6, 1, 1, 1, 1),
(2, 7, 1, 1, 1, 1),
(2, 8, 0, 0, 0, 0),
(3, 1, 0, 0, 0, 0),
(3, 2, 0, 0, 0, 0),
(3, 3, 1, 1, 1, 1),
(3, 4, 0, 0, 0, 0),
(3, 5, 0, 0, 0, 0),
(3, 6, 0, 0, 0, 0),
(3, 7, 0, 0, 0, 0),
(3, 8, 0, 0, 0, 0),
(4, 1, 1, 0, 0, 0),
(4, 2, 0, 0, 0, 0),
(4, 3, 1, 0, 0, 0),
(4, 4, 0, 0, 0, 0),
(4, 5, 0, 0, 0, 0),
(4, 6, 0, 0, 0, 0),
(4, 7, 0, 0, 0, 0),
(4, 8, 0, 0, 0, 0),
(5, 1, 0, 0, 0, 0),
(5, 2, 1, 0, 0, 0),
(5, 3, 1, 0, 0, 0),
(5, 4, 0, 0, 0, 0),
(5, 5, 0, 0, 0, 0),
(5, 6, 0, 0, 0, 0),
(5, 7, 1, 0, 0, 0),
(5, 8, 0, 0, 0, 0);

-- --------------------------------------------------------

--
-- Table structure for table `phieuhuy`
--

CREATE TABLE `phieuhuy` (
  `MAPHIEUHUY` int(11) NOT NULL,
  `MANHANVIEN` int(11) NOT NULL COMMENT 'Nhân viên báo hủy',
  `MANGUYENLIEU` int(11) NOT NULL COMMENT 'Nguyên liệu bị hủy',
  `SOLUONG` decimal(10,3) NOT NULL COMMENT 'Số lượng đã quy đổi ra Đơn vị cơ sở',
  `LYDO` varchar(255) DEFAULT NULL COMMENT 'Ví dụ: Làm đổ, Hết hạn, Pha sai',
  `NGAYTAO` datetime NOT NULL DEFAULT current_timestamp(),
  `MAHOADON` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `phieunhap`
--

CREATE TABLE `phieunhap` (
  `MAPN` int(11) NOT NULL,
  `MANCC` int(11) NOT NULL,
  `MANHANVIEN` int(11) NOT NULL,
  `THOIGIAN` datetime NOT NULL DEFAULT current_timestamp(),
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 0,
  `TONGTIEN` decimal(12,2) NOT NULL DEFAULT 0.00,
  `TRANGTHAIXOA` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `phieunhap`
--

INSERT INTO `phieunhap` (`MAPN`, `MANCC`, `MANHANVIEN`, `THOIGIAN`, `TRANGTHAI`, `TONGTIEN`, `TRANGTHAIXOA`) VALUES
(1, 1, 1, '2025-09-09 16:22:50', 1, 600000.00, 1),
(2, 1, 2, '2025-10-14 16:33:51', 1, 20000.00, 1),
(3, 1, 2, '2025-10-17 15:01:32', 1, 7500000.00, 1),
(4, 2, 1, '2025-11-05 14:51:54', 1, 3500000.00, 1),
(5, 4, 2, '2025-11-14 14:59:42', 1, 5040000.00, 1),
(6, 5, 1, '2025-11-22 15:03:29', 1, 4250000.00, 1),
(7, 10, 1, '2025-12-11 15:13:09', 1, 1500000.00, 1),
(8, 7, 1, '2025-12-12 13:39:19', 1, 73500000.00, 1);

-- --------------------------------------------------------

--
-- Table structure for table `quyen`
--

CREATE TABLE `quyen` (
  `MAQUYEN` int(11) NOT NULL,
  `TENQUYEN` varchar(100) NOT NULL,
  `TRANGTHAI` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `quyen`
--

INSERT INTO `quyen` (`MAQUYEN`, `TENQUYEN`, `TRANGTHAI`) VALUES
(1, 'Quản lý sản phẩm', 1),
(2, 'Quản lý nhập xuất', 1),
(3, 'Quản lý bán hàng', 1),
(4, 'Quản lý nhân sự', 1),
(5, 'Quản lý tài khoản', 1),
(6, 'Quản lý khách hàng', 1),
(7, 'Quản lý nhà cung cấp', 1),
(8, 'Quản lý phân quyền', 1);

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
(4, 3, 's-1-lon-cocacola.png', 'Coca cola lon', 1, 1, 15000.00),
(5, 1, 'sp_20251114153830_4b348a.jpg', 'abc', 0, 0, 2000000.00),
(6, 4, 'sp_20251212134014_dcab98.png', 'Cơm sườn', 1, 1, 40000.00),
(7, 3, 'sp_20251212134508_362edf.jpg', 'Cơm chiên gà', 1, 1, 50000.00);

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
  `NGAYTAO` datetime NOT NULL DEFAULT current_timestamp(),
  `MAVAITRO` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `taikhoan`
--

INSERT INTO `taikhoan` (`MATAIKHOAN`, `MANHANVIEN`, `TENDANGNHAP`, `MATKHAU`, `TRANGTHAI`, `NGAYTAO`, `MAVAITRO`) VALUES
(1, 1, 'admin', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 1, '2025-12-05 14:44:52', 1),
(2, 2, 'nhanvien1', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 1, '2025-12-05 14:50:35', 2),
(3, 4, 'nhanvien2', '96cae35ce8a9b0244178bf28e4966c2ce1b8385723a96a6b838858cdd6ca0a1e', 1, '2025-12-05 16:18:46', 5),
(4, 10, 'nhanven3', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 1, '2025-12-11 14:17:20', 4);

-- --------------------------------------------------------

--
-- Table structure for table `thanhtoan`
--

CREATE TABLE `thanhtoan` (
  `MATT` int(11) NOT NULL,
  `HINHTHUC` varchar(60) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `thanhtoan`
--

INSERT INTO `thanhtoan` (`MATT`, `HINHTHUC`) VALUES
(1, 'Tiền mặt'),
(2, 'Chuyển khoản'),
(3, 'Quẹt thẻ');

-- --------------------------------------------------------

--
-- Table structure for table `vaitro`
--

CREATE TABLE `vaitro` (
  `MAVAITRO` int(11) NOT NULL,
  `TENVAITRO` varchar(60) NOT NULL,
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `vaitro`
--

INSERT INTO `vaitro` (`MAVAITRO`, `TENVAITRO`, `TRANGTHAI`) VALUES
(1, 'Admin', 1),
(2, 'Quản lý', 1),
(3, 'Thu ngân', 1),
(4, 'Phục vụ', 1),
(5, 'Kế toán', 1);

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
  ADD PRIMARY KEY (`MAHOADON`,`MASANPHAM`),
  ADD KEY `FK_CTHD_SP` (`MASANPHAM`);

--
-- Indexes for table `ctphieunhap`
--
ALTER TABLE `ctphieunhap`
  ADD PRIMARY KEY (`MAPN`,`MANGUYENLIEU`,`MADONVI`),
  ADD KEY `fk_ctpn_nl` (`MANGUYENLIEU`),
  ADD KEY `fk_ctpn_dv` (`MADONVI`);

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
  ADD UNIQUE KEY `uq_loai_ten` (`TENLOAI`),
  ADD KEY `FK_LOAI_NHOM` (`MANHOM`);

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
-- Indexes for table `nhom`
--
ALTER TABLE `nhom`
  ADD PRIMARY KEY (`MANHOM`);

--
-- Indexes for table `phanquyen`
--
ALTER TABLE `phanquyen`
  ADD PRIMARY KEY (`MAVAITRO`,`MAQUYEN`),
  ADD KEY `MAQUYEN` (`MAQUYEN`);

--
-- Indexes for table `phieuhuy`
--
ALTER TABLE `phieuhuy`
  ADD PRIMARY KEY (`MAPHIEUHUY`),
  ADD KEY `FK_PHIEUHUY_NV` (`MANHANVIEN`),
  ADD KEY `FK_PHIEUHUY_NL` (`MANGUYENLIEU`);

--
-- Indexes for table `phieunhap`
--
ALTER TABLE `phieunhap`
  ADD PRIMARY KEY (`MAPN`),
  ADD KEY `FK_PN_NCC` (`MANCC`),
  ADD KEY `FK_PN_NV` (`MANHANVIEN`);

--
-- Indexes for table `quyen`
--
ALTER TABLE `quyen`
  ADD PRIMARY KEY (`MAQUYEN`);

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
  ADD UNIQUE KEY `TENDANGNHAP` (`TENDANGNHAP`),
  ADD KEY `FK_TK_VAITRO` (`MAVAITRO`);

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
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `ban`
--
ALTER TABLE `ban`
  MODIFY `MABAN` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=41;

--
-- AUTO_INCREMENT for table `calam`
--
ALTER TABLE `calam`
  MODIFY `MACA` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `donvi`
--
ALTER TABLE `donvi`
  MODIFY `MADONVI` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `hoadon`
--
ALTER TABLE `hoadon`
  MODIFY `MAHOADON` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT for table `khachhang`
--
ALTER TABLE `khachhang`
  MODIFY `MAKHACHHANG` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `khuvuc`
--
ALTER TABLE `khuvuc`
  MODIFY `MAKHUVUC` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `loai`
--
ALTER TABLE `loai`
  MODIFY `MALOAI` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `nguyenlieu`
--
ALTER TABLE `nguyenlieu`
  MODIFY `MANGUYENLIEU` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `nhacungcap`
--
ALTER TABLE `nhacungcap`
  MODIFY `MANCC` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `nhanvien`
--
ALTER TABLE `nhanvien`
  MODIFY `MANHANVIEN` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `nhom`
--
ALTER TABLE `nhom`
  MODIFY `MANHOM` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `phieuhuy`
--
ALTER TABLE `phieuhuy`
  MODIFY `MAPHIEUHUY` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `phieunhap`
--
ALTER TABLE `phieunhap`
  MODIFY `MAPN` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `quyen`
--
ALTER TABLE `quyen`
  MODIFY `MAQUYEN` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `sanpham`
--
ALTER TABLE `sanpham`
  MODIFY `MASANPHAM` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `taikhoan`
--
ALTER TABLE `taikhoan`
  MODIFY `MATAIKHOAN` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `thanhtoan`
--
ALTER TABLE `thanhtoan`
  MODIFY `MATT` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `vaitro`
--
ALTER TABLE `vaitro`
  MODIFY `MAVAITRO` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

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
  ADD CONSTRAINT `fk_ctpn_dv` FOREIGN KEY (`MADONVI`) REFERENCES `donvi` (`MADONVI`),
  ADD CONSTRAINT `fk_ctpn_nl` FOREIGN KEY (`MANGUYENLIEU`) REFERENCES `nguyenlieu` (`MANGUYENLIEU`),
  ADD CONSTRAINT `fk_ctpn_pn` FOREIGN KEY (`MAPN`) REFERENCES `phieunhap` (`MAPN`) ON DELETE CASCADE;

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
-- Constraints for table `loai`
--
ALTER TABLE `loai`
  ADD CONSTRAINT `FK_LOAI_NHOM` FOREIGN KEY (`MANHOM`) REFERENCES `nhom` (`MANHOM`);

--
-- Constraints for table `nguyenlieu`
--
ALTER TABLE `nguyenlieu`
  ADD CONSTRAINT `nguyenlieu_ibfk_1` FOREIGN KEY (`MADONVICOSO`) REFERENCES `donvi` (`MADONVI`);

--
-- Constraints for table `phanquyen`
--
ALTER TABLE `phanquyen`
  ADD CONSTRAINT `phanquyen_ibfk_1` FOREIGN KEY (`MAVAITRO`) REFERENCES `vaitro` (`MAVAITRO`) ON DELETE CASCADE,
  ADD CONSTRAINT `phanquyen_ibfk_2` FOREIGN KEY (`MAQUYEN`) REFERENCES `quyen` (`MAQUYEN`) ON DELETE CASCADE;

--
-- Constraints for table `phieuhuy`
--
ALTER TABLE `phieuhuy`
  ADD CONSTRAINT `FK_PHIEUHUY_NL` FOREIGN KEY (`MANGUYENLIEU`) REFERENCES `nguyenlieu` (`MANGUYENLIEU`),
  ADD CONSTRAINT `FK_PHIEUHUY_NV` FOREIGN KEY (`MANHANVIEN`) REFERENCES `nhanvien` (`MANHANVIEN`);

--
-- Constraints for table `phieunhap`
--
ALTER TABLE `phieunhap`
  ADD CONSTRAINT `FK_PN_NCC` FOREIGN KEY (`MANCC`) REFERENCES `nhacungcap` (`MANCC`),
  ADD CONSTRAINT `FK_PN_NV` FOREIGN KEY (`MANHANVIEN`) REFERENCES `nhanvien` (`MANHANVIEN`);

--
-- Constraints for table `sanpham`
--
ALTER TABLE `sanpham`
  ADD CONSTRAINT `FK_SANPHAM_LOAI` FOREIGN KEY (`MALOAI`) REFERENCES `loai` (`MALOAI`);

--
-- Constraints for table `taikhoan`
--
ALTER TABLE `taikhoan`
  ADD CONSTRAINT `FK_TAIKHOAN_NHANVIEN` FOREIGN KEY (`MANHANVIEN`) REFERENCES `nhanvien` (`MANHANVIEN`),
  ADD CONSTRAINT `FK_TK_VAITRO` FOREIGN KEY (`MAVAITRO`) REFERENCES `vaitro` (`MAVAITRO`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
