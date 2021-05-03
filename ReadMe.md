# Walmart Api Client

## Preparing for api request headers
1. Create an Walmart.io affiliate account. 
    - Go to walmart.io.
    - Click `Get Started` button. Or go to this [page](https://walmart.io/onboarding).
    - Go to the bottom of the page and click `Begin Creating Your Account`.
    - Sign in using your walmart account.
2. Create an application.
    - Go to the dashboard [page](https://walmart.io/dashboard).
    - Click `Create Application` and fill details of your application that uses walmart affiliate api.
3. Create RSA private/public keys.(Only tested on windows)
    - Use `cmd` and `ssh-keygen` command to create RSA private/public keys.
4. Upload public key to your walmart application.
    - On your dashboard page click `Upload/Update public key`.
    - Copy the public key and paste on the key section.
        > ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABAQC2Qe49CenGnw+Sd3a5Db9adLCFaNysGYHRXDY1a+PHErbpnAgRwq+6plQGFvIkXfNK8KqjPVPfb9LatOcw7cH/1Z9ErJHleJouBNpx5JZ0Gl+XMKXK5QcXIJICBiLOLL0VljgFs8ei+gdRWek5E8ZVrd68G9dg0KltCRrXk3MMl+EbSl0Fb/YKiiOib72IbmJomVy44D1W943Js261SaAA/mkD0LUTmzSP4FoTsaPONR6zeGzaCJSb8V3cZjetud8cZlphthE4bUM/kCX8ezseQwBo2TRMGD+t6DiHbNCjxyJSwUZrIVFOLbV+FuItxXWMBdzfbaF1HLzAD0C7s4Vr
    - You will get a new `Consumer ID` and `Private Key Version`.
