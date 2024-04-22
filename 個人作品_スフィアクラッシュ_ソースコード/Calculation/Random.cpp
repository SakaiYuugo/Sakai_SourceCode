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


int Random::GetInt(int min, int max)
{
    std::uniform_int_distribution<> dist(min, max);
    return dist(Generator);
}

float Random::GetFloat()
{
    std::uniform_real_distribution<> dist(FLT_MIN, FLT_MAX);
    return static_cast<float>(dist(Generator));
}

float Random::GetFloat(float min, float max)
{
    std::uniform_real_distribution<> dist(min, max);
    return static_cast<float>(dist(Generator));
}
