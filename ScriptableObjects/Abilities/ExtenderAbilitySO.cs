using UnityEngine;
using System.Collections;
using PixelBattleText;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "ExtenderAbilitySO", menuName = "Scriptable Objects/ExtenderAbilitySO")]
public class ExtenderAbilitySO : AbilitySO
{
   
    [Header("Extender Settings")]
    public float colliderWidthIncrease = 2.1f;
    public float colliderHeightDecrease = 0.4f;

    private void OnEnable()
    {
        abilityType = AbilityType.Extender;
        abilityMode = AbilityMode.Timed;
    }

    public override IEnumerator Activate(BattyController batty, AbilityManager manager)
    {
        Debug.Log("EXTENDER: Activated!");
       // PixelBattleTextController.DisplayText("EXTENDER", textAnimation, batty.gameObject.transform.position);

        SpriteRenderer spriteRenderer = batty.gameObject.GetComponent<SpriteRenderer>();
        CapsuleCollider2D collider = batty.Collider;

        // Save original state

        Sprite originalSprite = spriteRenderer.sprite;
         
        Vector2 originalColliderSize = collider.size;

        // Apply transformed sprite
        if (transformation != null)
        {
            spriteRenderer.sprite = transformation;
            Debug.Log("Sprite changed!");
        }
        // Apply collider change
        collider.size = new Vector2(
            collider.size.x + colliderWidthIncrease,
            collider.size.y - colliderHeightDecrease
        );

        batty.bonusStateNoise?.PlayFeedbacks();

        Debug.LogFormat("Colliders have been changed from OG SIZE = {0}, to NEW SIZE = {1}", originalColliderSize, collider.size);

        // Determine duration
        float time = duration > 0 ? duration : batty.BonusTime;
        yield return new WaitForSeconds(time);

        // Revert
        spriteRenderer.sprite = originalSprite;
        collider.size = originalColliderSize;

        Debug.Log("EXTENDER: Finished");

        batty.ClearAbility();
    }

}
