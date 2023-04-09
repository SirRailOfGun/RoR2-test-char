using System;
using System.Collections.Generic;
using System.Text;
using DuskWing.Modules.Survivors;
using DuskWing.SkillStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace DuskWing.Modules
{
    [RequireComponent(typeof(NetworkedBodyAttachment))]
    public class HallExtention : NetworkBehaviour, INetworkedBodyAttachmentListener
    {
        private NetworkedBodyAttachment networkedBodyAttachment;
        public void OnAttachedBodyDiscovered(NetworkedBodyAttachment newNetworkedBodyAttachment, CharacterBody attachedBody)
        {
            if (NetworkServer.active)
            {
                //this.damageListener = attachedBody.gameObject.AddComponent<HallOfMirrorsPassiveAttatchment.DamageListener>();
                //this.damageListener.passiveController = this;
            }
        }
        private void Awake()
        {
            this._monitoredSkill = base.GetComponent<SkillLocator>().FindSkillByFamilyName("special");
        }
        private GenericSkill _monitoredSkill;
        private bool skillAvailable;
        public GenericSkill monitoredSkill
        {
            get
            {
                return this._monitoredSkill;
            }
            set
            {
                if (this._monitoredSkill == value)
                {
                    return;
                }
                this._monitoredSkill = value;
                int num = -1;
                if (this._monitoredSkill)
                {
                    SkillLocator component = this._monitoredSkill.GetComponent<SkillLocator>();
                    if (component)
                    {
                        num = component.GetSkillSlotIndex(this._monitoredSkill);
                    }
                }
                //this.SetSkillSlotIndexPlusOne((uint)(num + 1));
            }
        }

        [Command]
        private void CmdSetSkillAvailable(bool newSkillAvailable)
        {
            this.skillAvailable = newSkillAvailable;
        }

        private void FixedUpdate()
        {
            if (this.networkedBodyAttachment.hasEffectiveAuthority)
            {
                this.FixedUpdateAuthority();
            }
        }
        private float HallTimer;
        private void FixedUpdateAuthority()
        {
            Debug.Log("update?");
            bool flag = false;
            if (this.monitoredSkill)
            {
                flag = (this.monitoredSkill.stock > 0);
            }
            if (this.skillAvailable != flag)
            {
                this.skillAvailable = flag;
                if (!NetworkServer.active)
                {
                    //this.CallCmdSetSkillAvailable(this.skillAvailable);
                    this.CmdSetSkillAvailable(this.skillAvailable);
                }
            }
            if (this.skillAvailable)
            {
                if (HallTimer > 5f)
                {
                    Debug.Log("fire?");
                    GameObject projectilePrefab = new GameObject();
                    projectilePrefab = Modules.Projectiles.hologramPrefab;
                    HallTimer = 0;
                }
                else
                {
                    Debug.Log("timer?");
                    HallTimer += Time.deltaTime;
                }
            }
            else
            {
                HallTimer = 0;
            }
        }
    }
}
