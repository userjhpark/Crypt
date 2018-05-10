<?php
class HxCrypt
{
//==========================================================================
// Begin ADODB의 crypt.inc.php
//==========================================================================
    //암호화, 복호화 값 생성
    function keyED($txt,$encrypt_key)
    {
        $encrypt_key = md5($encrypt_key);
        $ctr=0;
        $tmp = "";
        for ($i=0;$i<strlen($txt);$i++){
        if ($ctr==strlen($encrypt_key)) $ctr=0;
        $tmp.= substr($txt,$i,1) ^ substr($encrypt_key,$ctr,1);
        $ctr++;
        }
        return $tmp;
    }
    //암호화
    function Encrypt($txt,$key)
    {
        srand((double)microtime()*1000000);
        $encrypt_key = md5(rand(0,32000));
        $ctr=0;
        $tmp = "";
        for ($i=0;$i<strlen($txt);$i++)
        {
            if ($ctr==strlen($encrypt_key)) $ctr=0;
            $tmp.= substr($encrypt_key,$ctr,1) .
            (substr($txt,$i,1) ^ substr($encrypt_key,$ctr,1));
            $ctr++;
        }
        return base64_encode($this->keyED($tmp,$key));
    }
    //복호화
    function Decrypt($txt,$key)
    {
        $txt = $this->keyED(base64_decode($txt),$key);
        $tmp = "";
        for ($i=0;$i<strlen($txt);$i++){
        $md5 = substr($txt,$i,1);
        $i++;
        $tmp.= (substr($txt,$i,1) ^ $md5);
        }
        return $tmp;
    }
    //랜덤 문자 생성
    function RandPass()
    {
        $randomPassword = "";
        srand((double)microtime()*1000000);
        for($i=0;$i<$this->rand_len;$i++)
        {
        $randnumber = rand(48,120);

        while (($randnumber >= 58 && $randnumber <= 64) || ($randnumber >= 91 && $randnumber <= 96))
        {
            $randnumber = rand(48,120);
        }

        $randomPassword .= chr($randnumber);
        }
        return $randomPassword;
    }
//==========================================================================
// End ADODB의 crypt.inc.php
//=========================================================================
}
?>