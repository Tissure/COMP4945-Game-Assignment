using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game {
    public class AudioHandler : MonoBehaviour{
        public AudioSource audioSource;
        public AudioClip ballHitPlayer;
        public AudioClip ballHitWall;
        public AudioClip ballGoal;

        public void setBallHitPlayer() {
            audioSource.clip = ballHitPlayer;
        }
        public void setBallHitWall() {
            audioSource.clip = ballHitWall;
        }
        public void setBallGoal() {
            audioSource.clip = ballGoal;
        }

        public void playOnce() {
            audioSource.PlayOneShot(audioSource.clip);
            audioSource.clip = null;
        }
    }
}
