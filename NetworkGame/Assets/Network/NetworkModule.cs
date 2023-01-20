using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkModule {
    public abstract class CustomNetworkModule {
        public abstract void initDefaultNetwork();
        public abstract bool Send(string payload);
        public abstract void Receive();
    }
}
