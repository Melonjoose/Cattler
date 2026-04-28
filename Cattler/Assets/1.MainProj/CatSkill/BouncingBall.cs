using UnityEngine;
using System.Collections;

public class BouncingBall : CatSkill
{
    public Vector3 offset = new Vector3(0, 2, 0);
    protected override void Start()
    {
        base.Start();
        itemObject.gameObject.SetActive(false);
    }

    public override void UseSkill()
    {
        base.UseSkill();
        Debug.Log("BouncingBall skill activated!");
        StartCoroutine(SkillSequence());
    }

    IEnumerator SkillSequence()
    {
        AudioManager.instance.PlaySFX("ClownHonk");
        yield return new WaitForSeconds(0.5f);
        GameObject ball = Instantiate(itemObject, this.transform.position + offset , Quaternion.identity);
        ball.gameObject.SetActive(true);
        ball.transform.localScale = Vector3.zero;
        RedBall ballCS = ball.GetComponent<RedBall>();
        ballCS.bouncingball = this;
    }
}
