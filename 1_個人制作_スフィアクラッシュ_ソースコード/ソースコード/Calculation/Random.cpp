#include "Random.h"

#include <climits>
#include <float.h>

std::mt19937 Random::Generator;

/***************************************************************************
[�T�v]
�^����������(�����Z���k�E�c�C�X�^�[)

[�߂�l]
void
***************************************************************************/
void Random::Init()
{
    std::random_device randomDevice;
    Generator = std::mt19937(randomDevice());
}

/***************************************************************************
[�T�v]
int�^�̗������擾

[�߂�l]
void
***************************************************************************/
int Random::GetInt()
{
    std::uniform_int_distribution<> dist(INT_MIN, INT_MAX);
    return dist(Generator);
}

/***************************************************************************
[�T�v]
int�^�̗������w��͈͓��Ŏ擾

[����]
int const& min �擾�����̍ŏ��l
int const& max �擾�����̍ő�l

[�߂�l]
void
***************************************************************************/
int Random::GetInt(int const& min, int const& max)
{
    std::uniform_int_distribution<> dist(min, max);
    return dist(Generator);
}

/***************************************************************************
[�T�v]
float�^�̗������擾

[�߂�l]
void
***************************************************************************/
float Random::GetFloat()
{
    std::uniform_real_distribution<> dist(FLT_MIN, FLT_MAX);
    return static_cast<float>(dist(Generator));
}

/***************************************************************************
[�T�v]
float�^�̗������w��͈͓��Ŏ擾

[����]
float const& min �擾�����̍ŏ��l
float const& max �擾�����̍ő�l

[�߂�l]
void
***************************************************************************/
float Random::GetFloat(float const& min, float const& max)
{
    std::uniform_real_distribution<> dist(min, max);
    return static_cast<float>(dist(Generator));
}
