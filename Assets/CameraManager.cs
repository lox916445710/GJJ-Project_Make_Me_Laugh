using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

    public class CameraManager : MonoBehaviour
    {
        public CinemachineVirtualCamera vCamera;
        public CinemachineConfiner2D confiner2D;
        public ImpulseController impulseController;
        public static CameraManager instance;
        private void Awake()
        {
            if (instance != null)
                Destroy(gameObject);
            instance = this;
            if (impulseController == null)
                impulseController = GetComponent<ImpulseController>();
        }
        private void Update()
        {

        }

        public void ChangeCameraBound(Collider2D collider)
        {
            confiner2D.m_BoundingShape2D = collider;
        }
        public void ChangeCameraFollow(Transform t)
        {
            vCamera.Follow = t;
        }
        /// <summary>
        /// 将相机跟随切换回玩家
        /// </summary>
        public void SetCameraFollowToPlayer()
        {

        }
    }

