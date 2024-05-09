#include "Random.h"

#include <climits>
#include <float.h>

std::mt19937 Random::Generator;

/***************************************************************************
[概要]
疑似乱数生成(メルセンヌ・ツイスター)

[戻り値]
void
***************************************************************************/
void Random::Init()
{
    std::random_device randomDevice;
    Generator = std::mt19937(randomDevice());
}

/***************************************************************************
[概要]
int型の乱数を取得

[戻り値]
void
***************************************************************************/
int Random::GetInt()
{
    std::uniform_int_distribution<> dist(INT_MIN, INT_MAX);
    return dist(Generator);
}

/***************************************************************************
[概要]
int型の乱数を指定範囲内で取得

[引数]
int const& min 取得乱数の最小値
int const& max 取得乱数の最大値

[戻り値]
void
***************************************************************************/
int Random::GetInt(int const& min, int const& max)
{
    std::uniform_int_distribution<> dist(min, max);
    return dist(Generator);
}

/***************************************************************************
[概要]
float型の乱数を取得

[戻り値]
void
***************************************************************************/
float Random::GetFloat()
{
    std::uniform_real_distribution<> dist(FLT_MIN, FLT_MAX);
    return static_cast<float>(dist(Generator));
}

/***************************************************************************
[概要]
float型の乱数を指定範囲内で取得

[引数]
float const& min 取得乱数の最小値
float const& max 取得乱数の最大値

[戻り値]
void
***************************************************************************/
float Random::GetFloat(float const& min, float const& max)
{
    std::uniform_real_distribution<> dist(min, max);
    return static_cast<float>(dist(Generator));
}
