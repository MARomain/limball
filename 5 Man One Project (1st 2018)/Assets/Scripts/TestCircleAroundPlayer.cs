﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCircleAroundPlayer : MonoBehaviour {

        [Range(0, 50)]
        public int segments = 50;
        [Range(0, 20)]
        public float xradius = 10;
        [Range(0, 20)]
        public float yradius = 10;
        LineRenderer line;

        public Color c1 = Color.yellow;
        public Color c2 = Color.yellow;

    void Start()
        {
            line = gameObject.GetComponent<LineRenderer>();
        //line.material = new Material(Shader.Find("Particles/Additive"));
        line.SetColors(c1, c2);

        line.SetVertexCount(segments + 1);
            line.useWorldSpace = false;
            CreatePoints();
        }

        void CreatePoints()
        {
            float x;
            float y;
            float z;

            float angle = 20f;

            for (int i = 0; i < (segments + 1); i++)
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
                z = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

                line.SetPosition(i, new Vector3(x, 0, z));

                angle += (360f / segments);
            }
        }
}
