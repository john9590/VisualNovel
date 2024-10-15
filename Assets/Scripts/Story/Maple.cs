using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace QVN.Story
{
    using Models;
    using DefaultUI;

    public class Maple : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D _left;
        [SerializeField]
        private Rigidbody2D _right;
        [SerializeField]
        private TalkDialogSetter _getTalk;
        [SerializeField]
        private TalkDialogSetter _setLeft;
        [SerializeField]
        private TalkDialogSetter _setRight;
        protected object _leftAnimation;
        protected object _rightAnimation;
        public void MeshCut() {
            _setLeft.gameObject.SetActive(true);
            _setRight.gameObject.SetActive(true);
            _setLeft.SetBreak(_getTalk.GetCurName(),_getTalk.GetCurDialog());
            _setRight.SetBreak(_getTalk.GetCurName(),_getTalk.GetCurDialog());
            _getTalk.gameObject.SetActive(false);
            _left.AddForce(new Vector2(-200.0f,200.0f));
            _right.AddForce(new Vector2(200.0f,200.0f));
            _leftAnimation = _left.gameObject.transform.DORotate(new Vector3(0,0,45),3).target;
            _rightAnimation = _right.gameObject.transform.DORotate(new Vector3(0,0,-45),3).target;
        }
        public void MeshCutEnd() {
            DOTween.Kill(_leftAnimation, true);
            DOTween.Kill(_rightAnimation, true);
            _left.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            _left.gameObject.transform.position = new Vector3(0,-5,90);
            _right.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            _right.gameObject.transform.position = new Vector3(0,-5,90);
            _setLeft.gameObject.SetActive(false);
            _setRight.gameObject.SetActive(false);
            _getTalk.gameObject.SetActive(true);
            _getTalk.GetComponent<CanvasGroup>().alpha = 0.0f;
            _getTalk.GetComponent<CanvasGroup>().DOFade(1.0f,0.5f);
        }
    }
}

