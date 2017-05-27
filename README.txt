Stage 1 Milestone for teodor5

[Visual Studio Setup]

Since you already have visual studio, I will share my confguration, and hope for the best.
I of course have the "Android development with .Net" xamarin plugin, but I also have the "Android Development with C++ plugin" because it seems to have more android sdk and emulator features. 

I am using the template from https://marketplace.visualstudio.com/items?itemName=EgorBogatov.UrhoSharpTemplates for the project.  The only wierd thing is that it has a separate Data directory for each platform, making updating the resources a pain.  I will try to remedy this for stage 2.  The good thing about this template is that I don't have to even touch the code for the specific platforms--I can simply change the code in the (Portable) project and it usually just works. 
I am also using the "Git Extension for Visual Studio" plugin.

[Android]
I have an android 4.4 (api version 19) phone that I have tested this with, and I have the android project set to export to that api version.  I have also tested apk export, and have included the package.  I think you can download extra api versions using the Android SDK Manager (One of the buttons to the right of the green compile arrow).
According to my phone, the program is using gles2. 

[Shader]

The existing shader already had cube mapping available so I started with their LitSolid.glsl and called it LitSolidRefract.glsl. It is in Data/Shaders/GLSL/.


Urho compiles its shaders on the fly, and uses two xml files to configure the process for each material.  The materials use technique files that describe the #defines and passes that the shader will be used with. The material xml has the initial parameters for the material, which can be later changed with Material.SetShaderParameter(...)

The shader I added uses the REFRACT define to enable the vertex and pixel shaders.  I also found a definition for the refract function, which is also included in the file.

The code works in the same way as the example code, with some changes to make it fit in with urho's setup.

//this is before both vector and fragment shaders and defines the uniforms and variables needed by the shader.=
#ifdef REFRACT
	#ifndef GL_ES
		varying vec4 vEyeVec;
	#else
		varying highp vec4 vEyeVec;
	#endif

	//uniforms for frensel fragment shader
	#ifdef COMPILEPS
		uniform float cRefractIndex;
		uniform vec3 cRefractColor;
	#endif

	//this is from the glsl 1.1 specification
	vec3 refract(vec3 I, vec3 N, float eta) {
		float k = 1.0 - eta * eta * (1.0 - dot(N, I) * dot(N, I));
		if (k < 0.0)
			return vec3(0.0, 0.0, 0.0); // or genDType(0.0)
		else
			return eta * I - (eta * dot(N, I) + sqrt(k)) * N;
	}
#endif

//This is in the vertex shader
        #ifdef ENVCUBEMAP
            vReflectionVec = worldPos - cCameraPos;
        #endif

		#ifdef REFRACT
			vEyeVec = normalize(vec4(cCameraPos - worldPos, GetDepth(gl_Position)));
		#endif

//And this is in the fragment shader
        #ifdef ENVCUBEMAP
            finalColor += cMatEnvMapColor * textureCube(sEnvCubeMap, reflect(vReflectionVec, normal)).rgb;
        #endif
		#ifdef REFRACT
			vec3 refractvec = refract(vEyeVec.xyz, normal, cRefractIndex);
			finalColor += cRefractColor * textureCube(sEnvCubeMap, refractvec).rgb;
		#endif


Note that this shader does not do Fresnel reflection approximation.  I have found a good article on it and I will try to incorporate a FRESNEL feature for stage 2.  This should allow for a "Fresnel Power" slider.

[Program]

The program itself displays two objects rather than just one because unless I went all the way to stage 2, there was no way to reasonably show off both cube mapping and refraction at the same time clearly.  I have the cube set to 50% texture and 50% skybox reflection, and the sphere is set to 100% refraction.  Both features can be enabled simultaneously by editing the material colors.

The camera can be controlled with the screen joystick I copied from the examples.  I did not really think it was worth it at this point to fully debug the touch input, but I should have that working for stage2.  Most of the code for the program comes from the examples, so I left the copyright notice.
