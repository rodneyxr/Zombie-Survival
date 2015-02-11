
var skid : SoundController;
var emiterFL : ParticleEmitter;
var emiterFR : ParticleEmitter;
var emiterRL : ParticleEmitter;
var emiterRR : ParticleEmitter;



function Update () {
	if(skid.emit == true){
		emiterFL.emit = true;
		emiterFR.emit = true;
		emiterRL.emit = true;
		emiterRR.emit = true;
	}else{
		emiterFL.emit = false;
		emiterFR.emit = false;
		emiterRL.emit = false;
		emiterRR.emit = false;
	}
}