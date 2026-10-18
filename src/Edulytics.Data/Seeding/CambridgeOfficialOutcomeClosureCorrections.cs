using System.Security.Cryptography;
using System.Text;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Allows only the exact learner-body replacements introduced by the reviewed
/// Cambridge official-outcome closure. Both old and incoming translation bodies
/// are pinned by SHA-256 so unrelated edits remain fail-closed.
/// </summary>
public static class CambridgeOfficialOutcomeClosureCorrections
{
    private sealed record ReviewedReplacement(
        string PriorBodySha256,
        string ReviewedBodySha256);

    private static readonly IReadOnlyDictionary<string, ReviewedReplacement> Replacements =
        new Dictionary<string, ReviewedReplacement>(StringComparer.Ordinal)
        {
            ["PED:CAMBRIDGE-INTL-MATH:L10:CORE:01:06:APPROXIMATION-AND-BOUNDS"] = new(
                "19cd722f47391e92817953f4a25d9f4bf701a527a579553a01da7465a57a8e21",
                "176720a16c02d304d04132105e596e7922f436a3d96bb841578a1f83f94c4894"),
            ["PED:CAMBRIDGE-INTL-MATH:L10:CORE:02:01:ALGEBRAIC-MANIPULATION"] = new(
                "8064906bf3c38dfb45d22c8e0061413f6fbed511b3c6fcf98095a70b0d687614",
                "d64fe0dbbdf67575e17791793d0d95e2059334c5a307a9affe6d5a583d6816b0"),
            ["PED:CAMBRIDGE-INTL-MATH:L10:CORE:02:03:SIMULTANEOUS-LINEAR-EQUATIONS"] = new(
                "8c8715c237e18183bd98c5ecfc2d27b0b755f72ad221a0c18f8e3139d4af2ed4",
                "c5b7cb198e4762197b43dc7dcb04334437071f87a17e9089f9df1ccc25781489"),
            ["PED:CAMBRIDGE-INTL-MATH:L10:CORE:02:06:FUNCTIONS-AND-NOTATION"] = new(
                "14885ba9240c7b24e4c0f43c99cda1c1bc3d2a836f61c6d53ce7afbaf391c080",
                "338f5b657f758833595444932df4991b1946380221cff6a8b2cb2433754b9449"),
            ["PED:CAMBRIDGE-INTL-MATH:L10:CORE:04:01:ANGLE-FACTS"] = new(
                "c736369e71278fe2c96e93fac3cd6143f4ad12c10a25ded2a5b5dffa75c11407",
                "d57190be88335923ce0a8e4949c857c47b98e4a7b91a551ab2581b664c95f11f"),
            ["PED:CAMBRIDGE-INTL-MATH:L10:CORE:06:04:THREE-DIMENSIONAL-PROBLEMS"] = new(
                "06aeff6958a5f25cebb94e7867339267e59f1f96d70396d793f5731ac739d3a7",
                "89b2229cfee676ac9b5c2683dbc1eed3e4099db35a7cb3f40532b7fe1d0d730b"),
            ["PED:CAMBRIDGE-INTL-MATH:L10:CORE:08:02:COMBINED-EVENTS"] = new(
                "8f8af90100335c40657b2c1f9f570b58fea1a47b38c9ba185e6f0fd6df044772",
                "73ed233462f5f069b7a2b20d818873fc98c26bbcd0d1083040e68f98e967bf5e"),
            ["PED:CAMBRIDGE-INTL-MATH:L10:CORE:08:04:EXPECTED-FREQUENCY"] = new(
                "732dc5f48845c981d96bedf800c2ac36b75da136fa638dfbef8f980a4dac6a26",
                "cea29acf4cd2c5b1b79cf6a8c45d4de5ccac2c78208d8c0554884e1d4f4680d7"),
            ["PED:CAMBRIDGE-INTL-MATH:L10:CORE:09:02:AVERAGES-AND-SPREAD"] = new(
                "fd8d8c09431c467833f05c4d1ff2a632bab216a616f44610f263de5f417c573a",
                "1079c55f50b1359a8e7c62cc137f80e9dbe2fe1170b05c36b04bd84ce7a94cea"),
            ["PED:CAMBRIDGE-INTL-MATH:L10:EXTENDED:01:06:APPROXIMATION-AND-BOUNDS"] = new(
                "4fc6da5aef5ae15c0d593452b18479dbe87423c54d0dcf5d4951d1701e041640",
                "7f995fcbb946c2043135a65ce763436d1980c53e5b4fe8d5aafa034326774957"),
            ["PED:CAMBRIDGE-INTL-MATH:L10:EXTENDED:02:01:ALGEBRAIC-MANIPULATION"] = new(
                "5063fa84570b1e6ca2f6452917f4770f620b1cafcd8f3946949d3ae8832a0c6d",
                "9844d3c8aa81e76ae6059ea413214beb7827f804b1a6b350d23e66106ff825d2"),
            ["PED:CAMBRIDGE-INTL-MATH:L10:EXTENDED:02:03:SIMULTANEOUS-LINEAR-EQUATIONS"] = new(
                "8071ad75be8cc5783a906e59b3e5394e140a21836ee6c2d49eec1d70363670bd",
                "e83a3cb801e3697e1338d4822d05077378086be9cd69493d3ce23e7060547cda"),
            ["PED:CAMBRIDGE-INTL-MATH:L10:EXTENDED:03:01:GRADIENT"] = new(
                "a6914410f91ed6901f9e86159f80897a43d4e86f9931c6619810b3c10c2e4d04",
                "3f650c374e3b381a092fce10f762013d845ac62b1632358b73b3337e77b25380"),
            ["PED:CAMBRIDGE-INTL-MATH:L10:EXTENDED:05:02:CIRCLE-MEASURES"] = new(
                "e452a5ea990b6b41e665d5ef25fe0b52cca83036dd55543405b5f69d68aed730",
                "989611d6b93bab58bdafe0b670938a7ebac56de888f870b79af7aa331306e1f9"),
            ["PED:CAMBRIDGE-INTL-MATH:L10:EXTENDED:06:02:SINE-COSINE-AND-TANGENT"] = new(
                "b29ac2506ca63370254d79c84b72f24d7583dcac25abc64ffa43a240d3c02df5",
                "9ed0f5322a93e9e9f04e2743d8565377f4866434c58ea3494833a345a48e6cce"),
            ["PED:CAMBRIDGE-INTL-MATH:L10:EXTENDED:07:02:VECTOR-ARITHMETIC"] = new(
                "fe09d49e5a74f56840457e8d78446b9b9148ee9729896939a7e02e3486b0d620",
                "f1e58e860f76059d1e233da448980c03c716bd5e1ae4e3f36c7de461f3b27fb3"),
            ["PED:CAMBRIDGE-INTL-MATH:L10:EXTENDED:08:03:TREE-DIAGRAMS"] = new(
                "037c020661e03145d875186530bbdf32b068e25dde962b0dbbe8affb1de6e366",
                "b7d5ca2bb2269cb6a69e5288a2f0bbd6d01bd2261b76ce294aed3c08e35418cc"),
            ["PED:CAMBRIDGE-INTL-MATH:L10:EXTENDED:08:04:EXPECTED-FREQUENCY"] = new(
                "ad2925f0b3caf00986560208f33ef5a6e2300f811e54dca0226bfc174248efe7",
                "fb10bb839647bc10d23726ef0ad9f730bb36855218631f3cc8292dd2abd2031f"),
            ["PED:CAMBRIDGE-INTL-MATH:L10:EXTENDED:09:02:AVERAGES-AND-SPREAD"] = new(
                "08d28044a0a0efbb1d74dae69e2dc0da1341bed7763f2be9b2a068796dc2d368",
                "229ad776bef57d61696992fc4341ed690a48a2805dfe6f9dab6e0786d34e1a93"),
            ["PED:CAMBRIDGE-INTL-MATH:L11:EXTENDED:04:02:CONSOLIDATING-POLYGONS"] = new(
                "dfd361ca985dc627d0c66387b4a6c26da2cb71b35b92d2c2d15dc71850e84c5e",
                "d427dd382591c1f5104d90b882b22bc2c6ff8159be36679b8aa3bff7100edaa2"),
            ["PED:CAMBRIDGE-INTL-MATH:L12:COMPONENT-ROUTE-STRUCTURE-PRESERVED-IN-REFERENCE-GRAPH:01:01:ALGEBRAIC-MANIPULATION"] = new(
                "df2112a4604bead6f454cc0607be0af00af981f23166e4195595f07f22bba912",
                "5640078a3fc74ce8c4ece9eb1eb89038a34aec0de2048e704441e973a48f17c2"),
            ["PED:CAMBRIDGE-INTL-MATH:L12:COMPONENT-ROUTE-STRUCTURE-PRESERVED-IN-REFERENCE-GRAPH:01:02:QUADRATIC-FUNCTIONS"] = new(
                "eda188e9d4dc8cbc118edab34df9d4692d1c52e4e2224423a943a15b0cdc5ebb",
                "b6ff13f23609683d7558d124f0a74d6c2b68a6c44e8cefa5448aa3a9dda5f1b1"),
            ["PED:CAMBRIDGE-INTL-MATH:L12:COMPONENT-ROUTE-STRUCTURE-PRESERVED-IN-REFERENCE-GRAPH:01:03:FUNCTIONS-AND-GRAPHS"] = new(
                "ce2e1abb2cceb1352724f2d105608384ee8bc47e7542be9c0a0b8c2cea583934",
                "3951a37b7af4c10ac375541ca0d0b4b430a8c2347bf32c663340289b01949851"),
            ["PED:CAMBRIDGE-INTL-MATH:L12:COMPONENT-ROUTE-STRUCTURE-PRESERVED-IN-REFERENCE-GRAPH:01:07:TRIGONOMETRIC-EQUATIONS"] = new(
                "879cdcdbe669c1c1c33f5ba7db9440bda1d793a8d94a40a867adeb3347f62d88",
                "ee664135e51807a641a5f6eab29446215efdf6280fe3b42b54936a2784001204"),
            ["PED:CAMBRIDGE-INTL-MATH:L12:COMPONENT-ROUTE-STRUCTURE-PRESERVED-IN-REFERENCE-GRAPH:01:11:APPLICATIONS-OF-DIFFERENTIATION"] = new(
                "55b21ec670da6df0ef464d67df44d8cf19681186991be5325136405815f48907",
                "b9d3107c5750dda0f448c2cfa5164b75569d77e63c410c04d33d7552b6a562c6"),
            ["PED:CAMBRIDGE-INTL-MATH:L12:COMPONENT-ROUTE-STRUCTURE-PRESERVED-IN-REFERENCE-GRAPH:01:13:APPLICATIONS-OF-INTEGRATION"] = new(
                "b5a3c944b44f3defd4a77abca6fe9be64d4a14e8f2ffc0b4b4eda78606279966",
                "42fb9af38bfab6693d86badc5c694a3a9d75e344d9dc4d33ebb9d5acc451b91c"),
            ["PED:CAMBRIDGE-INTL-MATH:L7:SHARED:01:02:FRACTIONS-AND-MIXED-NUMBERS"] = new(
                "965386fd742fa2c2dfa7449cc7a57a08cedf16df022a945eda12d6698a397606",
                "51ebc1dbda3523c2a4c9b3267c307b97df837a97742b5084d16716823f718d78"),
            ["PED:CAMBRIDGE-INTL-MATH:L7:SHARED:01:07:POWERS-AND-ROOTS"] = new(
                "7db718d15679c36e800023ecacb3f1417ecc7c43ed1245f625f39c176085a1d7",
                "05825af3a80cb808f66fd18c0c9589667259c49f77f72fcb037d079a0ec81e94"),
            ["PED:CAMBRIDGE-INTL-MATH:L7:SHARED:02:02:SUBSTITUTION"] = new(
                "cbd841ba3e088b050d0f13054f2b96ff79cbaa59fa297ca3ef749dd412120a86",
                "b6b19ca98ec8692e7efea64f65bda9b1c2d69a41946097e732fd40922f348f20"),
            ["PED:CAMBRIDGE-INTL-MATH:L7:SHARED:02:03:EXPANDING-BRACKETS"] = new(
                "aa30d22a657db792afd38a9f08e7a36f99009015d42795373e4fbc387db3035f",
                "01ed7541fba4a7f09ade911d841665523eefee9a18facc62a03d32bc24524069"),
            ["PED:CAMBRIDGE-INTL-MATH:L7:SHARED:03:03:CONGRUENCE-AND-SIMILARITY"] = new(
                "7d41bcf7f3b1945727b1cf795589d494be7e3056426313a6b9fd7961cbffebd8",
                "64751f8434655f38dece23696f286222d20899dac6b620b475238ac830479efd"),
            ["PED:CAMBRIDGE-INTL-MATH:L7:SHARED:03:06:SURFACE-AREA-AND-VOLUME"] = new(
                "56431efff536823be7c0b2f9283589f1417e871982b0bb81c22367cfc3a1ae8d",
                "2aad34ecbd10d1eb66419dbf1c32aadcc711f6fca7322d555f53a3d046e2d5c1"),
            ["PED:CAMBRIDGE-INTL-MATH:L7:SHARED:04:05:EXPERIMENTAL-PROBABILITY"] = new(
                "fba47d3b445ae5b708043a9b787f3c43bfc47f85b5f39b58987b409ba9c6076b",
                "bed477f064b228c055c58c372ce21dc0d04d38d20b4234f55268b9c8ddf33a02"),
            ["PED:CAMBRIDGE-INTL-MATH:L8:SHARED:01:01:INTEGERS-AND-DIRECTED-NUMBER"] = new(
                "ecb313e0c0f20152aa7b7630795a175dd1073aa6b4e1a89255e42a9fc59c0080",
                "b014de3c4d6f3aac78b898e2d79e6aaba5e05947631ba2d7a4a8deff4fa0ad91"),
            ["PED:CAMBRIDGE-INTL-MATH:L8:SHARED:01:07:POWERS-AND-ROOTS"] = new(
                "76d36ff7a42d59bd62103f989be638f14d466d5a07cf0e17310c2956df07b89e",
                "0efb30f869ca8e1cb272508c06062c07ae9a86935d2cb1fe3ac7ab43f90c5d73"),
            ["PED:CAMBRIDGE-INTL-MATH:L8:SHARED:02:02:SUBSTITUTION"] = new(
                "6501b3fffe1ce9a223160b0549079fd6fc2bb17248b27e2cf3671135fa64819e",
                "2ca205a2cbb3dcfa20be16e5974aa9a93edca3f6ddb089b356690d8275e2eb70"),
            ["PED:CAMBRIDGE-INTL-MATH:L8:SHARED:02:09:SIMULTANEOUS-EQUATIONS-FOUNDATIONS"] = new(
                "6e1e3684519941a1bac95a11f8d782b8392ea1c77b738b856963622f642d712e",
                "6a6cc2b3d6f222b273766c778879f8242d7698e6ee38ef3c3bef4d126964b86d"),
            ["PED:CAMBRIDGE-INTL-MATH:L8:SHARED:03:06:SURFACE-AREA-AND-VOLUME"] = new(
                "0102432217360eebf705bcb49bcb93e366e692a36110886343aafdb7fbdd8c5e",
                "d2a11ebea1f3c5463400383cd7c3d95ad5019694cb4fc7a4e725e122a3c1b3a3"),
            ["PED:CAMBRIDGE-INTL-MATH:L8:SHARED:04:07:SAMPLE-SPACES"] = new(
                "67fb3b4cd95d309615db64c3b5653011c1fe35a4470d4aa060f5d5771ece5b20",
                "bd25faf77d745a22ba09236ad0e98e9d4f0cb9315ab972dee5b7d772e6abc2a9"),
            ["PED:CAMBRIDGE-INTL-MATH:L9:SHARED:01:07:POWERS-AND-ROOTS"] = new(
                "7de3fb97c3d07e9e738c58a6d03a704c826a4802f5be024a3051ec266b257f30",
                "e4f4d022bcb1705fdcbf7a4040ca07d394734de0bc115a2e316215220219a8de"),
            ["PED:CAMBRIDGE-INTL-MATH:L9:SHARED:02:01:ALGEBRAIC-EXPRESSIONS"] = new(
                "1a7c4a595efd94acdbbc1eebfe9f6aa37762cd1d41b076778eaffbf128b18474",
                "7b171a54ddb36b383476bf9ce43ec3101e8b5fb267644606be4e957b1320ccd8"),
            ["PED:CAMBRIDGE-INTL-MATH:L9:SHARED:02:09:QUADRATIC-EXPRESSIONS"] = new(
                "49a6dd33d02e1baf2c6e53d8dca713adbed6bef4e8f09956753be730b7e8a655",
                "03e09f26ba01627df18aa3a9559fbd2adce297a67e4bf5dede29595a2c792aa6"),
            ["PED:CAMBRIDGE-INTL-MATH:L9:SHARED:03:06:SURFACE-AREA-AND-VOLUME"] = new(
                "9110102cab0b87f25409ae13be146173597261b028393b858731e954e2676dcd",
                "b5ff6cb9495763b7c97b13e13178f9886a1ea2c58af49b6007605586b4891dec"),
            ["PED:CAMBRIDGE-INTL-MATH:L9:SHARED:04:07:SAMPLE-SPACES"] = new(
                "587512fe6131e831c893359670dd7fa991f73f17efae3594ae19dddfa1b89a8c",
                "d17266b99cfb2129ccf707b7c10fe7f55d495e5e903af3987063b91bf992d69f"),
            ["PED:CAMBRIDGE-INTL-MATH:S2:2G-1:APPLY"] = new(
                "666d132754cb6b31248b304e97fc7f06cae0f9069ea7843ce8ded48900fdbb64",
                "a50e03021027997a69f39bf2ac5388d9ddf69225634f97dbbd37ab96ddce9bcb"),
            ["PED:CAMBRIDGE-INTL-MATH:S2:2MD-1:APPLY"] = new(
                "56ad8700be6d1653bca2a78ef9bcb7c05875d6ec500d48c0d77fd8f91c90b400",
                "8a5e65f744bd7f876c56caac984bc701fbe768fa52225f050e873aff702e0dba"),
            ["PED:CAMBRIDGE-INTL-MATH:S2:2MD-1:BUILD"] = new(
                "7b43c7283dd414a53fad8c815766de7756ca634d51f86d34f642c14d1110a9a2",
                "7a9cfd770d7c3db0e558539fcc9b6cd63be2239b22bc831adeeff613de2ffc6d"),
            ["PED:CAMBRIDGE-INTL-MATH:S2:2MD-2:APPLY"] = new(
                "58c16babd32545a6fb5ecb867f183ad0e9b152b207e8efa811b15fee47039352",
                "6c6ed8b02256acbdbd6ea15283108f6ee2f274e17cbbe0587ff9104574bc6f79"),
            ["PED:CAMBRIDGE-INTL-MATH:S2:2MD-2:BUILD"] = new(
                "8aa1bbe31c44e4c9b91e4cad89b5e59b0ec223958e531763956ac1957a54523f",
                "103dab8bd14a3eeb252ee07f0935a9a5699023cd07209b4a5e409ecd6ae0ad08"),
            ["PED:CAMBRIDGE-INTL-MATH:S2:2NPV-1:APPLY"] = new(
                "13481f5172b71a6afdcd1420aac0742cc5c20b5d108a728341662cc3e93dae20",
                "535505887290d0412e957da4b6720af7ffb454adf13d192dcac254ec2d3736d6"),
            ["PED:CAMBRIDGE-INTL-MATH:S2:2NPV-2:APPLY"] = new(
                "80730c684db97cee3a9d8ed744a87e23aac2e177a0f4f7732a0a97219184f29b",
                "26313a71b9c8c2720e881439ab4d6efacf24d4a032fb2ea57d5a5a474c39d1bf"),
            ["PED:CAMBRIDGE-INTL-MATH:S2:2NPV-2:BUILD"] = new(
                "213b59068c966d74c48e58254fac77cddadb8bc75b956b63ed9fc16d8b4e7d4e",
                "2bb789b3e1baad49911ac24d9e09fe45bc8bffe76692a2f4ac39a34d75e48146"),
            ["PED:CAMBRIDGE-INTL-MATH:S3:3F-1:APPLY"] = new(
                "f488b5ccdb44fef8f844e4ba771a262b435de20f35a428f3c812291e7f20e96b",
                "961f212a063688a72ac59ccdb09e2c1b04adbd6a8e2264cf9f0633a78501895a"),
            ["PED:CAMBRIDGE-INTL-MATH:S3:3F-3:APPLY"] = new(
                "eab668beaaa031b12b747529f59a07694067854b611b46668bb99cc44b2bd849",
                "2776a22ce5e836928d75bc0c12cfbc02c1daa6874a11fd90c1b00b016a4be3e8"),
            ["PED:CAMBRIDGE-INTL-MATH:S3:3F-3:BUILD"] = new(
                "2dbd921bbfcb8b0381a5788a4107d8d868892d89b54f11f47798598d2bb1e86a",
                "9e5a74c35d831642836825561864dcd25ac8054ffa222f2e5c4b9a58dac88c84"),
            ["PED:CAMBRIDGE-INTL-MATH:S3:3G-1:APPLY"] = new(
                "d6bd62424bbfafdbf3f18b7d482ef50a2867c1fd797d7adf3c7706495d425a51",
                "2bf04d415806eda3ba335ca2edd93fc1dd74143dd0190e9360969908ceb0c130"),
            ["PED:CAMBRIDGE-INTL-MATH:S3:3G-1:BUILD"] = new(
                "47d69912f08187a04063991ea5e6f2ffae8513c68ccf4af4741839661f508e42",
                "049256cb52f1a92b961c2081d5ba3d94fe56d7308e13fbc1f93a52041179376c"),
            ["PED:CAMBRIDGE-INTL-MATH:S3:3MD-1:APPLY"] = new(
                "5ebb8caa6e488ae97ffce87d33528738a54235c2b36b40c96cdc228584e4d0c3",
                "bdd857eb3779d4d6005d100b020533d33325ab5923c27b8df4932ad1244b833b"),
            ["PED:CAMBRIDGE-INTL-MATH:S3:3MD-1:BUILD"] = new(
                "13fb095e084da17ad83a383e735e23e47ec439d14ba4f71a285fd88dfa26bcaa",
                "53f5cf27e362083cf607a955400dbb1a34fa859adfc715ee09b856d2ab9c4e7d"),
            ["PED:CAMBRIDGE-INTL-MATH:S3:3NPV-2:APPLY"] = new(
                "98329c3a65660e0bb597832fceb8c562e0e02b4df9c5ad954a2251f654c278dd",
                "d02d4dc308549ba3b5b3fa281105b130e4f0b9e7f35ddf313006c51081ba9d81"),
            ["PED:CAMBRIDGE-INTL-MATH:S3:3NPV-3:APPLY"] = new(
                "cff35a1c549177ba7e6b8b7fa0e0667f4781f290e1cceebb3d75ce0fd3a1df0a",
                "ff21ff63f1e6111aade839f683a05aeed789f9991f2a1c047ec786d0c280d5ec"),
            ["PED:CAMBRIDGE-INTL-MATH:S4:4F-2:APPLY"] = new(
                "508b34696dcd09e05c320c8e2031b3e842d045376ca998eb0829d871818fb11f",
                "d44edcfdf2b566b039c3ca81e888b99548c4b4ffe0e184b43190f4ffb35ca107"),
            ["PED:CAMBRIDGE-INTL-MATH:S4:4F-2:BUILD"] = new(
                "3d7ac6a17a523aea89ebd830e062ff2fb230cd849a96fa9917683a559085f143",
                "943cb751e5ab8cb64c3675aa0eade7279dba272a26f0a83238bf3c59ea1aecd4"),
            ["PED:CAMBRIDGE-INTL-MATH:S4:4G-1:APPLY"] = new(
                "8a518e70c776813f4661d31b6ec4f1726641aa56204e62074030882235fb46f9",
                "fdb2ffaba157cde2b34f056df91ab5250e99a8340f764e14fb74df34e95eae74"),
            ["PED:CAMBRIDGE-INTL-MATH:S4:4G-1:BUILD"] = new(
                "1a64a22d6b7d37e70f0bb6342c3e8f3f7bb94d235dc543b0978a32468d5aa471",
                "1489ea62843ef91a99c3e13a40cb8d1d3d0e14841e9e89eaef1559e327c20c48"),
            ["PED:CAMBRIDGE-INTL-MATH:S4:4G-2:APPLY"] = new(
                "81471924f7a9c0bbc549c53009475accb0eafa44e8e204c31049f5ba1dba497a",
                "db3b956669a9d701c90bc069c63e19555c805307c57ad632ebb7060f1f86c5ae"),
            ["PED:CAMBRIDGE-INTL-MATH:S4:4G-3:APPLY"] = new(
                "9bc964949b6e833107defc2187fed0f5384de5f3e93b0a0e364d09d5ae2593a1",
                "12df0fe6cfaf6b9446125b9ee74d5bab9ac2974f8e458d8ee4b01ea94b53cd9e"),
            ["PED:CAMBRIDGE-INTL-MATH:S4:4MD-2:APPLY"] = new(
                "9ef09d8101a4a59ff730adf87ec7dfda28f159e6d8d8ea2878f559401cb827e0",
                "f4ff58be9a109d8c06f120461956acb767a5d972c4d6ef4dec57314f3caf5490"),
            ["PED:CAMBRIDGE-INTL-MATH:S4:4NPV-2:APPLY"] = new(
                "86f259d7c63bf2a1ff189db01a90dd49f77d68102dd661f327c35029caa89e1f",
                "39d89aa7b66ad1a3399df1aa93ae18586cf6e27d7ad22290da4edb51fdad22aa"),
            ["PED:CAMBRIDGE-INTL-MATH:S4:4NPV-3:APPLY"] = new(
                "2c76e18323eedd6747bf59b8c701e57462db30f260a136dced5ca6adb6b889fb",
                "62d039e94f67a6f891865e962d30c67cff64ec7e513538728be4852a47a00a64"),
            ["PED:CAMBRIDGE-INTL-MATH:S5:5F-3:APPLY"] = new(
                "4d7c8dfdc5b31391d04f4f882a5437a6d6035104cb542a93b6186145fb592381",
                "6f848f7fe4d9ac364d766398480fe53f0b4c4fc598ce7af93de5198ce95f9d4a"),
            ["PED:CAMBRIDGE-INTL-MATH:S5:5F-3:BUILD"] = new(
                "e1adce8cc0636f92e9d22f6b5aedef621ccb0558c259c514a7e7f75132167bf2",
                "3fb336d77407e92061d9156969f8473defdebcc0d78defa5b3c108e246c82227"),
            ["PED:CAMBRIDGE-INTL-MATH:S5:5G-2:APPLY"] = new(
                "01f9fa60780697945a50e8cc5a6500130b34ec81d9fa18fe238f5b1d1b749991",
                "6cd1f63bd630e907e208690325ea61ac179eb5c3143bd5294512134d20544d38"),
            ["PED:CAMBRIDGE-INTL-MATH:S5:5MD-2:APPLY"] = new(
                "e2781b6044210b414fce76817b688f9f5d53a32d45ab7cbc8f8c8fe99238b97b",
                "0a33f1abb53b386c860866be750185b14afdac10011f09cd4f0f9944ccc0e785"),
            ["PED:CAMBRIDGE-INTL-MATH:S5:5MD-3:APPLY"] = new(
                "aed1c172a45d75fc93d6f2e3f5ef358247d5bcea5700f009ec2b52aee4f0d755",
                "608a576dc1cc715bec0191ac5e5901ca279513a7536fc1fde0734da2be2817fe"),
            ["PED:CAMBRIDGE-INTL-MATH:S5:5MD-3:BUILD"] = new(
                "ff756b3fe01a7e892a0c319d16b8abe8c58215ed7327e71611bef519ad8bae59",
                "07b2b26d7eb9eb6ffcdbe0fa124d67897e4b6b99a593a6e3eade54e10801f671"),
            ["PED:CAMBRIDGE-INTL-MATH:S5:5MD-4:APPLY"] = new(
                "a634362a7f1d01b21ecb23f7d7cd9c4561e24c994b1d6b1677a05c7c1d3de534",
                "a88bad1b107710bfd311a71f7b01040764fb236ea871a3f144efe9ac96d11900"),
            ["PED:CAMBRIDGE-INTL-MATH:S5:5NF-1:APPLY"] = new(
                "04a15714658d1a63cde27ae5bb863bc17a289f5e7e50a6bbd6031a1cb418242e",
                "6a02d91376017e656c64f251304f716a665581d4a6cf030a7ff1afd211bc8bed"),
            ["PED:CAMBRIDGE-INTL-MATH:S5:5NPV-3:APPLY"] = new(
                "35442ae435c7ab13f81d728c0b12cfa1cb3528b149a1850fcf0db50efe663768",
                "046fe138582563673d100f5f0addfd14b600b02f7bc2e4a6f36a64622749ff03"),
            ["PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-1:APPLY"] = new(
                "afb6e78ef0afcdccf48d625d75867ca8b1e4e56c4d8f0ffded2c429890511ede",
                "34827d3c32e7bb5fe3397ab13496d58af9146ed7c9e2e2132b57d2a2fe9897b9"),
            ["PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-2:APPLY"] = new(
                "a5a014b340648db32b8fca655c06f37639b5f14da4a1c868cd922ed45d5c2d79",
                "64a36f5c63370bae6d70e556dca16ff67c0c6a664ad323181c2221e8bb1e0943"),
            ["PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-2:BUILD"] = new(
                "8d124d41f7b946933c4b1f238a2fd915929e27e5276d69c4739264701420e874",
                "0bf16886ad1e7c6e5177bf4e889d15f356faf6d8f13c525f55f0644912ba637e"),
            ["PED:CAMBRIDGE-INTL-MATH:S6:6F-1:APPLY"] = new(
                "d1a9675326fabc7868456b6bf2cb0747d8e6b19042597f658129aa30afc634f9",
                "74cbda3da82df974d5fe7fdacf749c59066b0cc5bf73129f82d3da2ffebe21ed"),
            ["PED:CAMBRIDGE-INTL-MATH:S6:6F-1:BUILD"] = new(
                "86ef5e468016e2a9e0c1e230ec12a95fc06a47ff74f9902414bc1cd58d8bf0f3",
                "b0e33d88fb337274d7bcc670320dde874278d3a5412d2ddf404f237ccfeb2692"),
            ["PED:CAMBRIDGE-INTL-MATH:S6:6F-2:APPLY"] = new(
                "0d3b175f64cf09b17c4c5b4d173a7c536373c5e9a0df1d126e3aa0464d8eb890",
                "b645fc56b3bb8d204eb29e0110dd323e9c80b590fbdb99a0467991d28557dcff"),
            ["PED:CAMBRIDGE-INTL-MATH:S6:6G-1:APPLY"] = new(
                "f4b96f444931a02f2640228f4e8763ca3a091f81cc6aed6096740fdb57d5c813",
                "25d3994804e95cba552bc956ab17e0c2a3e186b595fd6d07e6b74f3c99f26f45"),
            ["PED:CAMBRIDGE-INTL-MATH:S6:6NPV-2:APPLY"] = new(
                "e9a88bbb0004b0b3b80216994896ffb322311df471fbd2bc8af183c92492b925",
                "0014b840c4c0a6245c755a0b54abffd6af288f1b3753eae9a0a2b8ef30b1780a"),
        };

    public static bool CanReplaceExistingBody(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        CurriculumLessonContentTranslation current,
        CanonicalLessonContentPackTranslation incoming)
    {
        if (document.PackCode != MathematicsCurriculumPackRegistry.CambridgeCode ||
            !Replacements.TryGetValue(lesson.LessonCode, out var replacement) ||
            !string.Equals(current.CultureCode, "en", StringComparison.Ordinal) ||
            !string.Equals(incoming.CultureCode, "en", StringComparison.Ordinal) ||
            !string.Equals(current.CultureCode, incoming.CultureCode, StringComparison.Ordinal))
        {
            return false;
        }

        if (!string.Equals(
                ComputeBodySha256(incoming),
                replacement.ReviewedBodySha256,
                StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Unreviewed Cambridge official-outcome closure body: {lesson.LessonCode}.");
        }

        return string.Equals(
            ComputeBodySha256(current),
            replacement.PriorBodySha256,
            StringComparison.Ordinal);
    }

    private static string ComputeBodySha256(CurriculumLessonContentTranslation translation) =>
        ComputeBodySha256(translation.CultureCode, translation.Title, translation.Explanation, translation.KeyConceptsAndRules, translation.WorkedExamples, translation.StepByStepSolutions, translation.CommonMistakes, translation.QuickSummary);

    private static string ComputeBodySha256(CanonicalLessonContentPackTranslation translation) =>
        ComputeBodySha256(translation.CultureCode, translation.Title, translation.Explanation, translation.KeyConceptsAndRules, translation.WorkedExamples, translation.StepByStepSolutions, translation.CommonMistakes, translation.QuickSummary);

    private static string ComputeBodySha256(params string[] values) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(string.Join("\n", values) + "\n"))).ToLowerInvariant();
}