#pragma once

#include <DirectXMath.h>
#include <initializer_list>
#include <array>
#include <algorithm>
#include <cmath>
#include <cassert>
#include <limits>


//-------------------------------------------------------
// XMFLOAT3
//-------------------------------------------------------

// XMFLOAT3���m�̌v�Z ------------------------------------
static inline void operator+= (DirectX::XMFLOAT3& v1, const DirectX::XMFLOAT3& v2)
{
    v1.x += v2.x;
    v1.y += v2.y;
    v1.z += v2.z;
}

static inline void operator-= (DirectX::XMFLOAT3& v1, const DirectX::XMFLOAT3& v2)
{
    v1.x -= v2.x;
    v1.y -= v2.y;
    v1.z -= v2.z;
}

static inline void operator*= (DirectX::XMFLOAT3& v1, const DirectX::XMFLOAT3& v2)
{
    v1.x *= v2.x;
    v1.y *= v2.y;
    v1.z *= v2.z;
}

static inline void operator/= (DirectX::XMFLOAT3& v1, const DirectX::XMFLOAT3& v2)
{
    v1.x /= v2.x;
    v1.y /= v2.y;
    v1.z /= v2.z;
}

static inline void operator%= (DirectX::XMFLOAT3& v1, const DirectX::XMFLOAT3& v2)
{
    v1.x = ::fmodf(v1.x, v2.x);
    v1.y = ::fmodf(v1.y, v2.y);
    v1.z = ::fmodf(v1.z, v2.z);
}

static inline constexpr auto operator+ (const DirectX::XMFLOAT3& v1, const DirectX::XMFLOAT3& v2)
{
    return DirectX::XMFLOAT3{ v1.x + v2.x, v1.y + v2.y, v1.z + v2.z };
}

static inline constexpr auto operator- (const DirectX::XMFLOAT3& v1, const DirectX::XMFLOAT3& v2)
{
    return DirectX::XMFLOAT3{ v1.x - v2.x, v1.y - v2.y, v1.z - v2.z };
}

static inline constexpr auto operator* (const DirectX::XMFLOAT3& v1, const DirectX::XMFLOAT3& v2)
{
    return DirectX::XMFLOAT3{ v1.x * v2.x, v1.y * v2.y, v1.z * v2.z };
}

static inline constexpr auto operator/ (const DirectX::XMFLOAT3& v1, const DirectX::XMFLOAT3& v2)
{
    return DirectX::XMFLOAT3{ v1.x / v2.x, v1.y / v2.y, v1.z / v2.z };
}

static inline constexpr auto operator% (const DirectX::XMFLOAT3& v1, const DirectX::XMFLOAT3& v2)
{
    return DirectX::XMFLOAT3{ ::fmodf(v1.x, v2.x), ::fmodf(v1.y, v2.y), ::fmodf(v1.z, v2.z) };
}



// XMFLOAT3��float�̌v�Z ---------------------------------
static inline void operator+= (DirectX::XMFLOAT3& v1, const float num)
{
    v1.x += num;
    v1.y += num;
    v1.z += num;
}

static inline void operator-= (DirectX::XMFLOAT3& v1, const float num)
{
    v1.x -= num;
    v1.y -= num;
    v1.z -= num;
}

static inline void operator*= (DirectX::XMFLOAT3& v1, const float num)
{
    v1.x *= num;
    v1.y *= num;
    v1.z *= num;
}

static inline void operator/= (DirectX::XMFLOAT3& v1, const float num)
{
    v1.x /= num;
    v1.y /= num;
    v1.z /= num;
}

static inline void operator%= (DirectX::XMFLOAT3& v1, const float num)
{
    v1.x = ::fmodf(v1.x, num);
    v1.y = ::fmodf(v1.y, num);
    v1.z = ::fmodf(v1.z, num);
}

static inline void operator+= (const float num, DirectX::XMFLOAT3& v1)
{
    v1.x += num;
    v1.y += num;
    v1.z += num;
}

static inline void operator-= (const float num, DirectX::XMFLOAT3& v1)
{
    v1.x -= num;
    v1.y -= num;
    v1.z -= num;
}

static inline void operator*= (const float num, DirectX::XMFLOAT3& v1)
{
    v1.x *= num;
    v1.y *= num;
    v1.z *= num;
}

static inline void operator/= (const float num, DirectX::XMFLOAT3& v1)
{
    v1.x /= num;
    v1.y /= num;
    v1.z /= num;
}

static inline void operator%= (const float num, DirectX::XMFLOAT3& v1)
{
    v1.x = ::fmodf(v1.x, num);
    v1.y = ::fmodf(v1.y, num);
    v1.z = ::fmodf(v1.z, num);
}

static inline constexpr auto operator+ (const DirectX::XMFLOAT3& v1, const float num)
{
    return DirectX::XMFLOAT3{ v1.x + num, v1.y + num, v1.z + num };
}

static inline constexpr auto operator- (const DirectX::XMFLOAT3& v1, const float num)
{
    return DirectX::XMFLOAT3{ v1.x - num, v1.y - num, v1.z - num };
}

static inline constexpr auto operator* (const DirectX::XMFLOAT3& v1, const float num)
{
    return DirectX::XMFLOAT3{ v1.x * num, v1.y * num, v1.z * num };
}

static inline constexpr auto operator% (const DirectX::XMFLOAT3& v1, const float num)
{
    return DirectX::XMFLOAT3{ ::fmodf(v1.x, num), ::fmodf(v1.y, num), ::fmodf(v1.z, num) };
}

static inline constexpr auto operator/ (const DirectX::XMFLOAT3& v1, const float num)
{
    return DirectX::XMFLOAT3{ v1.x / num, v1.y / num, v1.z / num };
}

// ��r --------------------------------------------------
static inline constexpr bool operator== (const DirectX::XMFLOAT3& v1, const DirectX::XMFLOAT3& v2)
{
    if (v1.x != v2.x) return false;
    if (v1.y != v2.y) return false;
    if (v1.z != v2.z) return false;

    return true;
}

// XMFLOAT�̑S�Ăɓ����l���� ----------------------------
static const inline auto XMFLOAT3_Assign(const float num)
{
    DirectX::XMFLOAT3 temp{ num, num, num };
    return temp;
}

// ��Βl�@-----------------------------------------------
static const inline auto Abs(DirectX::XMFLOAT3 v)
{
    v.x = std::abs(v.x);
    v.y = std::abs(v.y);
    v.z = std::abs(v.z);
    return v;
}

static const inline auto Abs(float num)
{
    num = std::abs(num);
    return num;
}

// �������@-----------------------------------------------
static inline auto Sqrt(const DirectX::XMFLOAT3& vf3)
{
    DirectX::XMFLOAT3 temp{};
    temp.x = sqrtf(vf3.x);
    temp.y = sqrtf(vf3.y);
    temp.z = sqrtf(vf3.z);
    return temp;
}

static inline auto Sqrt(const float& vf3)
{
    float temp{};
    temp = sqrtf(vf3);
    return temp;
}

// ���K���@-----------------------------------------------
static inline auto VectorNormalize(const DirectX::XMFLOAT3& vf3)
{
    DirectX::XMFLOAT3 rv{ vf3 };
    auto&& vec{ DirectX::XMLoadFloat3(&rv) };
    DirectX::XMStoreFloat3(&rv, DirectX::XMVector3Normalize(vec));
    return rv;
}

// �x�N�g���̒������擾 -----------------------------------
static inline auto VectorLength(const DirectX::XMFLOAT3 vf3)
{
    float len{};
    auto&& vec{ DirectX::XMLoadFloat3(&vf3) };
    DirectX::XMStoreFloat(&len, DirectX::XMVector3Length(vec));
    return len;
}

// �x�N�g���̓��ς��v�Z -----------------------------------
static inline auto VectorDot(const DirectX::XMFLOAT3& vec1, const DirectX::XMFLOAT3& vec2)
{
    float rv{};
    const auto&& v1{ DirectX::XMLoadFloat3(&vec1) }, && v2{ DirectX::XMLoadFloat3(&vec2) };
    DirectX::XMStoreFloat(&rv, DirectX::XMVector3Dot(v1, v2));
    return rv;
}

static inline auto Dot(const DirectX::XMFLOAT3& vec1, const DirectX::XMFLOAT3& vec2)
{
    DirectX::XMFLOAT3 temp;
    temp.x = vec1.x * vec2.x;
    temp.y = vec1.y * vec2.y;
    temp.z = vec1.z * vec2.z;
    return temp.x + temp.y + temp.z;
}

// �x�N�g���̊O�ς��v�Z -----------------------------------
static inline auto VectorCross(const DirectX::XMFLOAT3& vec1, const DirectX::XMFLOAT3& vec2)
{
    float rv{};
    const auto&& v1{ DirectX::XMLoadFloat3(&vec1) }, && v2{ DirectX::XMLoadFloat3(&vec2) };
    DirectX::XMStoreFloat(&rv, DirectX::XMVector3Cross(v1, v2));
    return rv;
}

static inline auto Cross(const DirectX::XMFLOAT3& vec1, const DirectX::XMFLOAT3& vec2)
{
    DirectX::XMFLOAT3 temp;
    temp.x = (vec1.y * vec2.z) - (vec1.z * vec2.y);
    temp.y = (vec1.z * vec2.x) - (vec1.x * vec2.z);
    temp.z = (vec1.x * vec2.y) - (vec1.y * vec2.x);
    return temp;
}