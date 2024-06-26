#include "Random.h"

#include <climits>
#include <float.h>

std::mt19937 Random::Generator;

/***************************************************************************
[Tv]
^Ά¬(ZkEcCX^[)

[ίθl]
void
***************************************************************************/
void Random::Init()
{
    std::random_device randomDevice;
    Generator = std::mt19937(randomDevice());
}

/***************************************************************************
[Tv]
int^ΜπζΎ

[ίθl]
void
***************************************************************************/
int Random::GetInt()
{
    std::uniform_int_distribution<> dist(INT_MIN, INT_MAX);
    return dist(Generator);
}

/***************************************************************************
[Tv]
int^ΜπwθΝΝΰΕζΎ

[ψ]
int const& min ζΎΜΕ¬l
int const& max ζΎΜΕεl

[ίθl]
void
***************************************************************************/
int Random::GetInt(int const& min, int const& max)
{
    std::uniform_int_distribution<> dist(min, max);
    return dist(Generator);
}

/***************************************************************************
[Tv]
float^ΜπζΎ

[ίθl]
void
***************************************************************************/
float Random::GetFloat()
{
    std::uniform_real_distribution<> dist(FLT_MIN, FLT_MAX);
    return static_cast<float>(dist(Generator));
}

/***************************************************************************
[Tv]
float^ΜπwθΝΝΰΕζΎ

[ψ]
float const& min ζΎΜΕ¬l
float const& max ζΎΜΕεl

[ίθl]
void
***************************************************************************/
float Random::GetFloat(float const& min, float const& max)
{
    std::uniform_real_distribution<> dist(min, max);
    return static_cast<float>(dist(Generator));
}
