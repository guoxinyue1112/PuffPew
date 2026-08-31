using UnityEngine;

public class AxeWeapon : WeaponBase
{
    private const float Range = 2.5f;
    private const float AnimationDuration = 0.18f;

    private SpriteRenderer swingRenderer;
    private Coroutine swingRoutine;

    protected override void Awake()
    {
        base.Awake();
        baseDamage = 25f;
        baseAttackInterval = 1.2f;
        CreateSwingVisual();
    }

    protected override bool TryAttack()
    {
        Enemy target = GetNearestEnemy();
        float rangeSqr = Range * Range;
        bool hitAnyEnemy = false;
        var activeEnemies = GameManager.Instance.EnemyManager.ActiveEnemies;
        float damage = GetDamage();
        Vector3 swingTargetPosition = target != null
            ? target.transform.position
            : transform.position + transform.up;

        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            Enemy enemy = activeEnemies[i];
            if (enemy == null)
            {
                continue;
            }

            if ((enemy.transform.position - transform.position).sqrMagnitude > rangeSqr)
            {
                continue;
            }

            if (!hitAnyEnemy)
            {
                swingTargetPosition = enemy.transform.position;
                hitAnyEnemy = true;
            }

            enemy.TakeDamage(damage);
        }

        PlaySwing(swingTargetPosition);
        return hitAnyEnemy;
    }

    protected override bool ShouldUseCooldownOnMiss()
    {
        return true;
    }

    private void CreateSwingVisual()
    {
        GameObject swingObject = new("AxeSwingVisual");
        swingObject.transform.SetParent(transform, false);
        swingObject.transform.localPosition = Vector3.zero;

        swingRenderer = swingObject.AddComponent<SpriteRenderer>();
        Sprite axeSprite = PuffPewArt.GetAxeSprite();
        swingRenderer.sprite = axeSprite != null ? axeSprite : RuntimeSpriteFactory.GetSquareSprite();
        swingRenderer.color = axeSprite != null ? Color.white : new Color(1f, 0.88f, 0.48f, 0.92f);
        swingRenderer.sortingOrder = 5;
        swingRenderer.enabled = false;

        if (axeSprite != null)
        {
            PuffPewArt.SetUniformWorldSize(swingObject.transform, axeSprite, 1.8f);
        }
        else
        {
            swingObject.transform.localScale = new Vector3(0.28f, 1.8f, 1f);
        }
    }

    private void PlaySwing(Vector3 targetPosition)
    {
        if (swingRenderer == null)
        {
            return;
        }

        Vector2 direction = (targetPosition - transform.position).normalized;
        float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        if (swingRoutine != null)
        {
            StopCoroutine(swingRoutine);
        }

        swingRoutine = StartCoroutine(SwingRoutine(baseAngle));
    }

    private System.Collections.IEnumerator SwingRoutine(float baseAngle)
    {
        Transform swingTransform = swingRenderer.transform;
        swingRenderer.enabled = true;

        float elapsed = 0f;
        while (elapsed < AnimationDuration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / AnimationDuration);
            float sweepAngle = Mathf.Lerp(-70f, 70f, progress);
            swingTransform.localRotation = Quaternion.Euler(0f, 0f, baseAngle + sweepAngle);
            swingTransform.localPosition = swingTransform.up * 1.05f;

            Color color = swingRenderer.color;
            color.a = Mathf.Lerp(0.95f, 0f, progress);
            swingRenderer.color = color;
            yield return null;
        }

        swingTransform.localPosition = Vector3.zero;
        swingTransform.localRotation = Quaternion.identity;
        swingRenderer.enabled = false;
        Color resetColor = swingRenderer.color;
        resetColor.a = 0.92f;
        swingRenderer.color = resetColor;
        swingRoutine = null;
    }
}
