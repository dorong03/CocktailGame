using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

static class ScoreCalculator
{
    private static float[] bonusTime = { 5f, 3f, 2f, 1f, 0f };
    

    // 각 재료별 정확도 
    private static float GetIngredientAccuracy(float targetAmount, float actualAmount)
    {
        float ingredientAccuracy = Mathf.Max(0, 1 - (Mathf.Abs(actualAmount - targetAmount) / targetAmount));

        return ingredientAccuracy;
    }

    // 재료 정확도의 평균값
    public static float GetMixAccuracy(RecipeData recipe, MixResult result)
    {
        Dictionary<string, float> targetMixes = new Dictionary<string, float>();

        int mixCount = result.Ingredients.Count;
        float totalAccuracy = 0f;

        foreach (var targetMix in recipe.Ingredients)
        {
            targetMixes.Add(targetMix.IngredientId, targetMix.Amount);
        }

        foreach(var actualMix in result.Ingredients)
        {
            // targetAmount 에 레시피 용량을 저장함
            targetMixes.TryGetValue(actualMix.IngredientId, out float targetAmount);
            // actualAmount 에 내가 담은 용량을 저장함
            float actualAmount = actualMix.Amount;
            totalAccuracy += GetIngredientAccuracy(targetAmount, actualAmount);
        }
        float finalAccuracy = totalAccuracy / mixCount;
        return finalAccuracy;
    }

    // 최종 점수
    public static int GetFinalScore(int baseScore, float accuracy)
    {
        // 레시피의 베이스 스코어와 재료 정확도의 점수, 시트 점수를 총함하여 보내기
        int convertAccuracy = Convert.ToInt32(accuracy);
        return baseScore * convertAccuracy;
    }

    // Grade 를 받아서 시간 반환해주기
    public static float GetBonusSeconds(Grade grade)
    {
        return bonusTime[(int)grade];
    }
}
