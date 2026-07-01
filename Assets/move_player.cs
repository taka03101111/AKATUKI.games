using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_player : MonoBehaviour
{
    Vector3 position; // 物体の位置を格納する変数
                      // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }
    // Update is called once per frame
    void Update()
    {
        position = transform.position; // 現在位置の保存
        if (Input.GetKey("up")) // ↑なら前(Z 方向)に 0.1 だけ進む
        {
            transform.position += transform.forward * 0.3f;
        }
        if (Input.GetKey("down")) // ↓なら-Z 方向に 0.1 だけ進む
        {
            transform.position -= transform.forward * 0.1f;
        }
        if (Input.GetKey("right")) // ←なら Y 軸に 3 度回転する
        {
            transform.Rotate(0f, 3.0f, 0f);
        }
        if (Input.GetKey("left")) // →なら Y 軸に-3 度回転する
        {
            transform.Rotate(0f, -3.0f, 0f);
        }
    }
    void OnCollisionEnter(Collision other) // 衝突を判定する関数を呼ぶ
    {
        Debug.Log("object =" + other.gameObject.name);
        if (other.gameObject.name != "床") // 「床」以外との衝突が判定された
        {
            Vector3 boundVec = transform.position - position; // 移動ベクトル
            Debug.Log("object =" + boundVec);
            transform.position = position - boundVec; // 押し戻す
        }
    }
}
